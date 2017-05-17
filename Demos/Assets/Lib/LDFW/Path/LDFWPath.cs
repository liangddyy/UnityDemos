using UnityEngine;
using System.Collections;


namespace LDFW.Math {

    [System.Serializable]
    public class LDFWPath {

        public AnimationCurve xValueCurve;
        public AnimationCurve yValueCurve;
        public AnimationCurve zValueCurve;

        public float totalDistance;

        public LDFWPath () {
            Reset ();
        }

        public void Reset () {
            xValueCurve = new AnimationCurve ();
            yValueCurve = new AnimationCurve ();
            zValueCurve = new AnimationCurve ();
        }
        
        public void AddKeyFrame (Vector3 position, float percentage) {
            percentage = Mathf.Max (0, Mathf.Min (1, percentage));
            xValueCurve.AddKey (percentage, position.x);
            yValueCurve.AddKey (percentage, position.y);
            zValueCurve.AddKey (percentage, position.z);
        }

        public Vector3 Evaluate (float percentage) {
            return new Vector3 (xValueCurve.Evaluate (percentage), yValueCurve.Evaluate (percentage), zValueCurve.Evaluate (percentage));
        }
    }

}