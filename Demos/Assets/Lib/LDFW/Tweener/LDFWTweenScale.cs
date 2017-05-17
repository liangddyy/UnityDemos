using UnityEngine;
using System.Collections;

namespace LDFW.Tweener {
    public class LDFWTweenScale : LDFWTweenBase {

        new void Awake () {
            base.Awake ();
        }

        protected override void PreStart () {
            startingValue = targetTransform.localScale;
            if (useCurrentValueAsStartingValue) {
                fromValue = targetTransform.localScale;
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
                targetTransform.localScale = currentValue;
            }
        }
    }

}