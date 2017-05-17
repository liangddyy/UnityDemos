using UnityEngine;
using System.Collections;

namespace LDFW.Tweener {
    [RequireComponent (typeof (RectTransform))]
    public class LDFWTweenUIAnchoredPosition : LDFWTweenBase {

        // Public Variables
        public RectTransform uiTargetTransform;

        // Private Variables
        private Vector2 tempVector = Vector2.zero;

        new void Awake () {
            base.Awake ();

            if (targetTransform == null) {
                targetTransform = GetComponent<Transform> ();
            }

            uiTargetTransform = targetTransform as RectTransform;
        }

        protected override void PreStart () {
            startingValue.x = uiTargetTransform.anchoredPosition.x;
            startingValue.y = uiTargetTransform.anchoredPosition.y;
            if (useCurrentValueAsStartingValue) {
                fromValue.x = uiTargetTransform.anchoredPosition.x;
                fromValue.y = uiTargetTransform.anchoredPosition.y;
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

        }

        public override void UpdateTweener () {
            if (accumulatedTime > startDelay) {
                tempVector.x = currentValue.x;
                tempVector.y = currentValue.y;

                uiTargetTransform.anchoredPosition = tempVector;
            }
        }
    }
}