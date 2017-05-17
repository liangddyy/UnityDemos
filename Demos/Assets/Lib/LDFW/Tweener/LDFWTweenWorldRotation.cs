using UnityEngine;
using System.Collections;

namespace LDFW.Tweener {
    public class LDFWTweenWorldRotation : LDFWTweenBase {

        new void Awake () {
            base.Awake ();
        }

        protected override void PreStart () {
            startingValue = targetTransform.rotation.eulerAngles;
            if (useCurrentValueAsStartingValue) {
                fromValue = targetTransform.rotation.eulerAngles;
            }
        }

        new void Start () {
            base.Start ();
        }

        new void Update () {
            if (!isTweenerPlaying) {
                if (playingFramesNumber <= 0) {
                    return;
                }
            }

            base.Update ();

            UpdateTweener ();
        }

        public override void UpdateTweener () {
            if (accumulatedTime > startDelay) {
                targetTransform.eulerAngles = currentValue;
            }
        }
    }

}