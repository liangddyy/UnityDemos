using UnityEngine;
using System.Collections;

namespace LDFW.Tweener {
    public class LDFWTweenCameraFieldOfView : LDFWTweenBase {

        private Camera targetCamera;

        new void Awake () {
            base.Awake ();
            targetCamera = GetComponent<Camera> ();
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
                return;
            }

            if (targetCamera == null) {
                return;
            }
            base.Update ();

            UpdateTweener ();
        }

        public override void UpdateTweener () {
            if (accumulatedTime > startDelay) {
                targetCamera.fieldOfView = currentValue.x;
            }
        }
    }

}