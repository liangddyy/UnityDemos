using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace LDFW.Tweener {

    public class LDFWTweenColor : LDFWTweenBase {

        // Public Variables
        public MaskableGraphic uiImageTarget = null;
        public Renderer rendererTarget = null;

        public bool useUIImage = false;
        public bool useRenderer = false;
        public bool useShader = false;
        public string shaderColorKey;

        public float alphaFromValue = 0f;
        public float alphaToValue = 0f;
        public AnimationCurve animationAlphaCurve = new AnimationCurve (new Keyframe (0f, 0f, 0f, 1f), new Keyframe (1f, 1f, 1f, 0f));


        // Private and Protected Variables
        private float alphaCurrentValue = 0f;
        private float alphaDiffValue = 0f;
        private Vector4 currentColorVector = Vector4.zero;


        new void Awake () {
            base.Awake ();

            // Tries to find Image, RawImage, Renderer components in this order
            if (uiImageTarget == null) {
                if ((uiImageTarget = GetComponent<Image> ()) == null) {
                    if ((uiImageTarget = GetComponent<RawImage> ()) == null) {
                        if (rendererTarget == null) {
                            rendererTarget = GetComponent<Renderer> ();
                        }
                    }
                }
            }

            // If no targets were found, stops all further processing
            if (uiImageTarget == null && rendererTarget == null) {
                autoPlay = isTweenerPlaying = false;
            } else if (!useUIImage && !useRenderer) {
                if (uiImageTarget != null) {
                    useUIImage = true;
                } else {
                    useRenderer = true;
                }
            }
        }

        protected override void PreStart () {
            if (useCurrentValueAsStartingValue) {
                if (useUIImage) {
                    fromValue.x = uiImageTarget.color.r;
                    fromValue.y = uiImageTarget.color.g;
                    fromValue.z = uiImageTarget.color.b;
                    alphaFromValue = uiImageTarget.color.a;
                } else {
                    fromValue.x = rendererTarget.material.color.r;
                    fromValue.y = rendererTarget.material.color.g;
                    fromValue.z = rendererTarget.material.color.b;
                    alphaFromValue = rendererTarget.material.color.a;
                }
            }
        }

        new void Start () {
            base.Start ();

            alphaDiffValue = alphaToValue - alphaFromValue;
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

        protected override void PostCurrentValueCalculation () {
            alphaCurrentValue = GetValueBasedOnAnimationCurve (animationAlphaCurve, alphaDiffValue, alphaFromValue);
        }

        public override void UpdateTweener () {
            if (accumulatedTime > startDelay) {
                if (useUIImage) {
                    currentColorVector.x = currentValue.x / 255f;
                    currentColorVector.y = currentValue.y / 255f;
                    currentColorVector.z = currentValue.z / 255f;
                    currentColorVector.w = alphaCurrentValue / 255f;
                    uiImageTarget.color = currentColorVector;
                } else {
                    currentColorVector.x = currentValue.x / 255f;
                    currentColorVector.y = currentValue.y / 255f;
                    currentColorVector.z = currentValue.z / 255f;
                    currentColorVector.w = alphaCurrentValue / 255f;

                    if (useShader) {
                        rendererTarget.material.SetColor (shaderColorKey, currentColorVector);
                    } else {
                        rendererTarget.material.color = currentColorVector;
                    }
                }
            }
        }
    }

}