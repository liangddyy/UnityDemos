using UnityEngine;
using System.Collections;

namespace LDFW.Tweener {
    public class LDFWTweenWorldPosition : LDFWTweenBase {

        new void Awake () {
            base.Awake ();
        }

        protected override void PreStart () {
            startingValue = targetTransform.position;
            if (useCurrentValueAsStartingValue) {
                fromValue = targetTransform.position;
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
                targetTransform.position = currentValue;
            }
        }
    }

}