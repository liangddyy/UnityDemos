using UnityEngine;
using UnityEngine.UI;
using System.Collections;


namespace LDFW.Tweener {

    public class LDFWTweenLayoutElement : LDFWTweenBase {

        public LayoutElement target;

        public enum LayoutElementTweenTarget {
            MinWidth,
            MinHeight,
            PreferredWidth,
            PreferredHeight,
            FlexibleWidth,
            FlexibleHeight
        }

        public LayoutElementTweenTarget layoutTweenTarget;

        new void Awake () {
            base.Awake ();
            if (target == null) {
                target = GetComponent<LayoutElement> ();
            }
        }

        private void SetTargetValue (float val) {
            switch (layoutTweenTarget) {
                case LayoutElementTweenTarget.MinWidth:
                    target.minWidth = val;
                    break;
                case LayoutElementTweenTarget.MinHeight:
                    target.minHeight = val;
                    break;
                case LayoutElementTweenTarget.PreferredWidth:
                    target.preferredWidth = val;
                    break;
                case LayoutElementTweenTarget.PreferredHeight:
                    target.preferredHeight = val;
                    break;
                case LayoutElementTweenTarget.FlexibleWidth:
                    target.flexibleWidth = val;
                    break;
                case LayoutElementTweenTarget.FlexibleHeight:
                    target.flexibleHeight = val;
                    break;
            }
        }

        private float GetTargetValue () {
            switch (layoutTweenTarget) {
                case LayoutElementTweenTarget.MinWidth:
                    return target.minWidth;
                case LayoutElementTweenTarget.MinHeight:
                    return target.minHeight;
                case LayoutElementTweenTarget.PreferredWidth:
                    return target.preferredWidth;
                case LayoutElementTweenTarget.PreferredHeight:
                    return target.preferredHeight;
                case LayoutElementTweenTarget.FlexibleWidth:
                    return target.flexibleWidth;
                case LayoutElementTweenTarget.FlexibleHeight:
                    return target.flexibleHeight;
                default:
                    return 0f;
            }
        }

        protected override void PreStart () {
            startingValue.x = GetTargetValue ();

            if (useCurrentValueAsStartingValue) {
                fromValue.x = GetTargetValue ();
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
                SetTargetValue (currentValue.x);
            }
        }

    }

}