using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace LDFW.Tweener {
    public abstract class LDFWTweenBase : MonoBehaviour {

        public enum TweenerStyle {
            Once,
            Loop,
            PingPong,
        }

        // Public Variables
        public Transform targetTransform = null;

        public bool autoPlay = false;
        public bool useCurrentValueAsStartingValue = false;
        public bool useRelativeValueBasedOnStartingValue = false;
        public bool generateRandomCurveBasedOnFromAndTo = false;

        public Vector3 fromValue = Vector3.zero;
        public Vector3 toValue = Vector3.zero;

        public AnimationCurve animationXCurve = new AnimationCurve (new Keyframe (0f, 0f, 0f, 1f), new Keyframe (1f, 1f, 1f, 0f));
        public AnimationCurve animationYCurve = new AnimationCurve (new Keyframe (0f, 0f, 0f, 1f), new Keyframe (1f, 1f, 1f, 0f));
        public AnimationCurve animationZCurve = new AnimationCurve (new Keyframe (0f, 0f, 0f, 1f), new Keyframe (1f, 1f, 1f, 0f));

        public TweenerStyle style = TweenerStyle.Once;

        public float startDelay = 0f;
        public float duration = 1f;
        public bool ignoreTimeScale = true;

        public UnityEvent tweenEndEvent = new UnityEvent ();

        // Private and protected variables
        protected Vector3 startingValue = Vector3.zero;
        protected Vector3 currentValue = Vector3.zero;
        protected Vector3 diffValue = Vector3.zero;
        protected float accumulatedTime = 0f;
        protected bool isPlayingReverse = false;
        protected bool isCurrentAnimationBackwards = false;
        protected bool isTweenerPlaying = false;

        protected IEnumerator burstTweenIEnumerator = null;
        protected int playingFramesNumber = 0;


        protected void Awake () {
            if (targetTransform == null) {
                targetTransform = transform;
            }
            //animationXCurve.
        }

        // PreStart is used to set the current values if any, it runs first in Start () method
        protected abstract void PreStart ();

        protected void Start () {
            PreStart ();
            Init ();

            if (autoPlay) {
                isTweenerPlaying = true;
            }
        }

        public LDFWTweenBase InitAutoDestroyGO (Vector3 start, Vector3 end, float time, float delay, bool autoPlay = false) {
            fromValue = start;
            toValue = end;
            duration = time;
            startDelay = delay;
            accumulatedTime = 0f;

            Init ();
            if (autoPlay) {
                PlayWithDelay ();
            }

            tweenEndEvent.AddListener (() => { gameObject.SetActive (false); Destroy (gameObject); });

            return this;
        }

        public LDFWTweenBase InitAutoDestroyComponent (Vector3 start, Vector3 end, float time, float delay, bool autoPlay = false) {
            fromValue = start;
            toValue = end;
            duration = time;
            startDelay = delay;
            accumulatedTime = 0f;

            Init ();
            if (autoPlay) {
                PlayWithDelay ();
            }

            tweenEndEvent.AddListener (() => { Destroy (this); });

            return this;
        }

        public LDFWTweenBase InitWithEndEvent (Vector3 start, Vector3 end, float time, float delay, UnityAction endEvent, bool autoPlay = false) {
            fromValue = start;
            toValue = end;
            duration = time;
            startDelay = delay;
            accumulatedTime = 0f;
            tweenEndEvent = new UnityEvent ();
            tweenEndEvent.AddListener (endEvent);

            Init ();
            if (autoPlay) {
                PlayWithDelay ();
            }

            return this;
        }

        public LDFWTweenBase Init (Vector3 start, Vector3 end, float time, float delay, bool autoPlay = false) {
            fromValue = start;
            toValue = end;
            duration = time;
            startDelay = delay;
            accumulatedTime = 0f;
            tweenEndEvent = new UnityEvent ();

            Init ();
            if (autoPlay) {
                PlayWithDelay ();
            }

            return this;
        }

        protected LDFWTweenBase Init () {
            accumulatedTime = 0f;

            if (useRelativeValueBasedOnStartingValue) {
                fromValue += startingValue;
                toValue += startingValue;
            }
            currentValue = fromValue;
            diffValue = toValue - fromValue;

            if (generateRandomCurveBasedOnFromAndTo) {
                GenerateRandomCurve (animationXCurve);
                GenerateRandomCurve (animationYCurve);
                GenerateRandomCurve (animationZCurve);
            }

            return this;
        }

        protected void GenerateRandomCurve (AnimationCurve curve, float slices = 10f) {
            while (curve.keys.Length > 0) {
                curve.RemoveKey (0);
            }

            for (int i = 0; i <= slices; i++) {
                curve.AddKey (i / slices, Random.Range (0f, 1f));
            }
        }

        protected void Update () {
            // only processes if isTweenerPlaying is true
            if (!isTweenerPlaying) {
                if (playingFramesNumber <= 0) {
                    return;
                } else {
                    playingFramesNumber--;
                }
            }

            // Update tweener
            CalculateCurrentValue ();
            
            // increments accumualtedTime;
            accumulatedTime += Time.deltaTime;
        }

        // Calcualtes current value
        protected virtual void CalculateCurrentValue () {
            // if there is a start delay, do nothing
            if (accumulatedTime < startDelay) {

            } else {
                // decrements accumualtedTime if it's greater than duration, meaning: next iteration has started
                if (accumulatedTime - startDelay > duration) {
                    accumulatedTime -= duration;
                    if (style == TweenerStyle.PingPong) {
                        isCurrentAnimationBackwards = !isCurrentAnimationBackwards;
                    } else if (style == TweenerStyle.Once) {
                        accumulatedTime = startDelay + duration;
                        isTweenerPlaying = false;
                        if (tweenEndEvent != null) {
                            tweenEndEvent.Invoke ();

                        }
                        //DisableTweener ();
                    }
                }

                float timeScale = 1;
                if (!ignoreTimeScale) {
                    timeScale = Time.timeScale;
                }

                // updates currentValue
                currentValue.x = GetValueBasedOnAnimationCurve (animationXCurve, diffValue.x, fromValue.x) * timeScale;
                currentValue.y = GetValueBasedOnAnimationCurve (animationYCurve, diffValue.y, fromValue.y) * timeScale;
                currentValue.z = GetValueBasedOnAnimationCurve (animationZCurve, diffValue.z, fromValue.z) * timeScale;
                PostCurrentValueCalculation ();
            }
        }

        // Updates tweener
        public virtual void UpdateTweener () {
            // base class does nothing
        }

        // runs right after currentValue has been calculated for this frame
        protected virtual void PostCurrentValueCalculation () {
            // default behaviour is to do nothing
        }

        protected virtual float GetValueBasedOnAnimationCurve (AnimationCurve curve, float diffValue, float fromValue) {
            float temp = (accumulatedTime - startDelay) / duration;
            if (isCurrentAnimationBackwards) {
                if (isPlayingReverse) {
                    return curve.Evaluate ((accumulatedTime - startDelay) / duration) * diffValue + fromValue;
                } else {
                    return curve.Evaluate ((duration - (accumulatedTime - startDelay)) / duration) * diffValue + fromValue;
                }
            } else {
                if (isPlayingReverse) {
                    return curve.Evaluate ((duration - (accumulatedTime - startDelay)) / duration) * diffValue + fromValue;
                } else {
                    return curve.Evaluate ((accumulatedTime - startDelay) / duration) * diffValue + fromValue;
                }
            }
        }

        #region notes
        /*
         *		SetToBeginning ()
         *			set accumulatedTime to startDelay value
         *		SetToEndding ()
         *			set accumulatedTime to startDelay value + duration value
         *		SetToPercentagePoint (float)
         *			set accumulated time to startDelay + duration * percentage
         *		PauseTweener ()
         *			set isTweenerPlaying to false
         *		ResumeTweener ()
         *			set isTweenerPlaying to true
         *		ResetTweener ()
         *			set accumulatedTime = 0
         * 		Play ()
         *			SetToBeginning ()
         *			set isPlayingReverse to false
         *			set isTweenPlaying to true
         * 		PlayReverse ()
         *			SetToBeginning ()
         *			set isPlayingReverse to true
         *			set isTweenPlaying to true
         */
        #endregion

        public void SetToBeginning () {
            accumulatedTime = startDelay;
        }

        public void SetToEndding () {
            accumulatedTime = startDelay + duration;
        }

        public void SetToPercentagePoint (float percent) {
            percent = Mathf.Clamp (percent, 0f, 1f);

            accumulatedTime = startDelay + duration * percent;
            CalculateCurrentValue ();
            UpdateTweener ();
        }

        public float GetCurrentPercentage () {
            return Mathf.Clamp ((accumulatedTime - startDelay) / duration, 0f, 1f);
        }

        public void PauseTweener () {
            isTweenerPlaying = false;
        }

        public void ResumeTweener () {
            isTweenerPlaying = true;
        }

        public void ResetTweener () {
            accumulatedTime = 0f;
            diffValue = toValue - fromValue;
        }

        public virtual void SwitchFromAndTo () {
            Vector3 temp = fromValue;
            fromValue = toValue;
            toValue = temp;
        }

        [ContextMenu ("PlayWithDelay")]
        public void PlayWithDelay () {
            Init ();
            isPlayingReverse = false;
            isTweenerPlaying = true;
        }

        [ContextMenu ("Play")]
        public void Play () {
            Init ();
            SetToBeginning ();
            isPlayingReverse = false;
            isTweenerPlaying = true;
        }

        [ContextMenu ("PlayReverse")]
        public void PlayReverse () {
            Init ();
            SetToBeginning ();
            isPlayingReverse = true;
            isTweenerPlaying = true;
        }

        public void BurstTweenBasedOnFrames (int framesNum) {
            playingFramesNumber = framesNum;
        }

        private IEnumerator BurstTweenBasedOnFramesCoroutine (int framesNum) {

            ResumeTweener ();
            for (int i = 0; i < framesNum; i++) {
                yield return null;
            }
            PauseTweener ();
            burstTweenIEnumerator = null;
        }

        public void BurstTweenBasedOnTime (float time) {
            if (burstTweenIEnumerator != null) {
                StopCoroutine (burstTweenIEnumerator);
            }

            burstTweenIEnumerator = BurstTweenBasedOnTimeCoroutine (time);
            StartCoroutine (burstTweenIEnumerator);
        }

        private IEnumerator BurstTweenBasedOnTimeCoroutine (float time) {
            ResumeTweener ();
            yield return new WaitForSeconds (time);
            PauseTweener ();
            burstTweenIEnumerator = null;
        }

    }

}