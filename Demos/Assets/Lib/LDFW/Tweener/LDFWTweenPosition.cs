using UnityEngine;
using System.Collections;

namespace LDFW.Tweener {
    public class LDFWTweenPosition : LDFWTweenBase {

        new void Awake () {
            base.Awake ();
        }

        protected override void PreStart () {
            startingValue = targetTransform.localPosition;

            if (useCurrentValueAsStartingValue) {
                fromValue = targetTransform.localPosition;
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
                targetTransform.localPosition = currentValue;
            }
        }
    }

}