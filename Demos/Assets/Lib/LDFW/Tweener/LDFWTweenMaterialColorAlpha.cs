using UnityEngine;
using System.Collections;

namespace LDFW.Tweener {
    public class LDFWTweenMaterialColorAlpha : LDFWTweenBase {

        public MeshRenderer meshRenderer = null;

        public float alphaFromValue = 0f;
        public float alphaToValue = 0f;

        public Color materialColor;

        new void Awake () {
            base.Awake ();
            meshRenderer = GetComponent<MeshRenderer> ();
            materialColor = meshRenderer.material.GetColor ("_Color");
        }

        protected override void PreStart () {
            fromValue.x = alphaFromValue;
            toValue.x = alphaToValue;
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
                materialColor.a = currentValue.x;
                meshRenderer.material.SetColor ("_Color", materialColor);
            }
        }
    }

}