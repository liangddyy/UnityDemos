using UnityEngine;
using System.Collections.Generic;


namespace Babybus.PathCreator
{
    [ExecuteInEditMode]
    public class CurvePathAnimator : MonoBehaviour
    {
        public CurvePath _pathObject;

        public CurvePath pathObject
        {
            get
            {
                if (_pathObject == null)
                    _pathObject = GetComponent<CurvePath>();
                return _pathObject;
            }
        }

        public bool isPlaying { get; private set; }

        public Transform animateObject;
        public bool animateSceneObjectInEditor;

        public float percent = 0;

        public float animationSpeed = 1;

        public enum AnimateMode
        {
            once,
            loop,
            reverse,
            reverseLoop,
            pingPong
        }

        public enum OrientationMode
        {
            lookAt,
            lerp,
            slerp,
            lookForward,
            none
        }

        public bool playOnAwake;

        public AnimateMode animateMode = AnimateMode.once;

        public OrientationMode orientationMode = OrientationMode.none;

        public Transform lookAtTarget;

        public Quaternion start, end;

        public delegate void AnimationStartedEventHandler();

        public delegate void AnimationPausedEventHandler();

        public delegate void AnimationStoppedEventHandler();

        public delegate void AnimationFinishedEventHandler();

        public delegate void AnimationLoopedEventHandler();

        public delegate void AnimationPingPongEventHandler();

        public event AnimationStartedEventHandler AnimationStartedEvent;
        public event AnimationPausedEventHandler AnimationPausedEvent;
        public event AnimationStoppedEventHandler AnimationStoppedEvent;
        public event AnimationFinishedEventHandler AnimationFinishedEvent;
        public event AnimationLoopedEventHandler AnimationLoopedEvent;
        public event AnimationPingPongEventHandler AnimationPingPongEvent;

        private int pingPongDirection = 1;

        private bool isReversed
        {
            get
            {
                return animateMode == AnimateMode.reverse || animateMode == AnimateMode.reverseLoop ||
                       (animateMode == AnimateMode.pingPong && pingPongDirection == -1);
            }
        }

        void Awake()
        {
            if (playOnAwake)
                isPlaying = true;
        }

        void Update()
        {
            if (!isPlaying)
                return;

            UpdateAnimation();
            UpdateAnimationTime();
        }

        void LateUpdate()
        {
            if (!isPlaying)
                return;
        }

        void UpdateAnimationTime()
        {
            switch (animateMode)
            {
                case AnimateMode.once:
                    if (percent < 1)
                    {
                        percent += Time.deltaTime * animationSpeed;
                        percent = Mathf.Clamp01(percent);

                        if (percent == 1)
                        {
                            isPlaying = false;
                            if (AnimationFinishedEvent != null)
                                AnimationFinishedEvent();
                        }
                    }
                    break;

                case AnimateMode.loop:
                    if (percent < 1)
                    {
                        percent += Time.deltaTime * animationSpeed;
                        percent = Mathf.Clamp01(percent);

                        if (percent == 1)
                        {
                            if (AnimationLoopedEvent != null)
                                AnimationLoopedEvent();

                            percent = 0;
                        }
                    }
                    break;

                case AnimateMode.reverse:
                    if (percent >= 0) // 原本是percent > 0，若起点跟终点一样的话，一直等于0，进不了if，就一直不stop
                    {
                        percent -= Time.deltaTime * animationSpeed;
                        percent = Mathf.Clamp01(percent);

                        if (percent == 0)
                        {
                            isPlaying = false;
                            if (AnimationFinishedEvent != null)
                                AnimationFinishedEvent();
                        }
                    }
                    break;

                case AnimateMode.reverseLoop:
                    if (percent > 0)
                    {
                        percent -= Time.deltaTime * animationSpeed;
                        percent = Mathf.Clamp01(percent);

                        if (percent == 0)
                        {
                            if (AnimationLoopedEvent != null)
                                AnimationLoopedEvent();

                            percent = 1;
                        }
                    }
                    break;

                case AnimateMode.pingPong:
                    float timeStep = Time.deltaTime * animationSpeed;
                    percent += timeStep * pingPongDirection;
                    percent = Mathf.Clamp01(percent);
                    if (percent == 1)
                    {
                        if (AnimationPingPongEvent != null)
                            AnimationPingPongEvent();

                        pingPongDirection = -1;
                    }

                    if (percent == 0)
                    {
                        if (AnimationPingPongEvent != null)
                            AnimationPingPongEvent();

                        pingPongDirection = 1;
                    }
                    break;
            }
        }

        void UpdateAnimation()
        {
            if (animateObject == null)
                return;

            if (pathObject == null)
                return;


            if (!Application.isPlaying && !animateSceneObjectInEditor)
                return;

            //Debug.Log(percent);
            animateObject.position = pathObject.GetPosition(percent);
            
            switch (orientationMode)
            {
                case OrientationMode.lookAt:
                    animateObject.LookAt(lookAtTarget);
                    break;

                case OrientationMode.lerp:
                    animateObject.rotation = Quaternion.Lerp(start, end, percent);
                    break;

                case OrientationMode.slerp:
                    animateObject.rotation = Quaternion.Slerp(start, end, percent);
                    break;

                case OrientationMode.lookForward:
                    int dir = 1;
                    if (animateMode == AnimateMode.reverse || animateMode == AnimateMode.reverseLoop ||
                        (animateMode == AnimateMode.pingPong && pingPongDirection == -1))
                        dir = -1;
                    if (percent + 0.01f * dir > 1 || percent + 0.01f * dir < 0)
                        break;
                    animateObject.LookAt(pathObject.GetPosition(percent + 0.01f * dir));
                    break;

                case OrientationMode.none:
                    break;
            }
        }

        public void Reverse()
        {
            switch (animateMode)
            {
                case AnimateMode.once:
                    animateMode = AnimateMode.reverse;
                    break;
                case AnimateMode.reverse:
                    animateMode = AnimateMode.once;
                    break;
                case AnimateMode.pingPong:
                    pingPongDirection *= -1;
                    break;
                case AnimateMode.loop:
                    animateMode = AnimateMode.reverseLoop;
                    break;
                case AnimateMode.reverseLoop:
                    animateMode = AnimateMode.loop;
                    break;
            }
        }

        public void Play()
        {
            if (isPlaying)
                return;

            isPlaying = true;
            if (!isReversed)
            {
                if (this.animateMode == AnimateMode.once && percent == 1)
                    percent = 0;
                if (percent == 0)
                {
                    if (AnimationStartedEvent != null)
                        AnimationStartedEvent();
                }
            }
            else
            {
                if (percent == 1)
                {
                    if (AnimationStartedEvent != null)
                        AnimationStartedEvent();
                }
            }
        }

        public void Stop()
        {
            if (!isPlaying)
                return;

            isPlaying = false;
            if (isReversed)
                percent = 1;
            else
                percent = 0;
            if (AnimationStoppedEvent != null)
                AnimationStoppedEvent();
        }

        public void Pause()
        {
            if (!isPlaying)
                return;

            isPlaying = false;
            if (AnimationPausedEvent != null)
                AnimationPausedEvent();
        }

        void OnDestroy()
        {
        }
    }
}