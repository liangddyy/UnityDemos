using UnityEngine;
using System.Collections;

namespace LDFW.Tweener {
    public class LDFWTweenUV : LDFWTweenBase {

        private Material material;

        public bool useSameXValueToWidthAndHeight = false;

        new void Awake () {
            base.Awake ();
            material = targetTransform.GetComponent<MeshRenderer> ().material;
        }

        protected override void PreStart () {
            if (useCurrentValueAsStartingValue) {
                fromValue.x = material.GetTextureScale ("_MainTex").x;
                fromValue.y = material.GetTextureScale ("_MainTex").y;
            }
        }

        new void Start () {
            base.Start ();
            useRelativeValueBasedOnStartingValue = false;
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

                if (useSameXValueToWidthAndHeight) {
                    material.SetTextureScale ("_MainTex", new Vector2 (currentValue.x, currentValue.x));
                    material.SetTextureOffset ("_MainTex", new Vector2 (-(currentValue.x - 1) * 0.5f, -(currentValue.x - 1) * 0.5f));
                } else {
                    material.SetTextureScale ("_MainTex", new Vector2 (currentValue.x, currentValue.y));
                    material.SetTextureOffset ("_MainTex", new Vector2 (-(currentValue.x - 1) * 0.5f, -(currentValue.y - 1) * 0.5f));
                }


            }
        }
    }

}