using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

namespace LDFW.Tweener {

    public class LDFWTweenUICanvasGroup : LDFWTweenBase {

        public CanvasGroup target;

        private UnityEvent callBack = null;

        new void Awake () {
            base.Awake ();
            if (target == null) {
                target = GetComponent<CanvasGroup> ();
            }
        }

        protected override void PreStart () {
            if (useCurrentValueAsStartingValue) {
                fromValue.x = target.alpha;
            }
        }

        new void Start () {
            base.Start ();
        }

        new void Update () {
            if (!isTweenerPlaying) {
                return;
            }
            base.Update ();

            UpdateTweener ();

            /*
            if (isTweenerPlaying) {
                base.Update ();
                target.alpha = currentValue.x;

                if (!isTweenerPlaying) {
                    if (callBack != null) {
                        callBack.Invoke ();
                    }
                }
            }
            */
        }

        /*
        public void PlayOnceCallBack (UnityEvent callback = null) {
            if (target != null) {
                callBack = callback;
                Play ();
            }
        }

        public void PlayOnceReverseCallBack (UnityEvent callback = null) {
            if (target != null) {
                callBack = callback;
                PlayReverse ();
            }
        }
        */

        public override void UpdateTweener () {
            if (accumulatedTime > startDelay) {
                target.alpha = currentValue.x;
            }
        }
    }

}