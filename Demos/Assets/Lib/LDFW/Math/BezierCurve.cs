using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LDFW.Math {

    public class BezierCurve {

        private List<Vector3> points;
        private Dictionary<int, Vector3> cachedPoints;
        private int precision;

        public BezierCurve () {
            Reset ();
        }
        
        public void Reset () {
            points = new List<Vector3> ();
            cachedPoints = new Dictionary<int, Vector3> ();
            precision = 0;
        }

        public void GenerateCache (int segmentCount) {
            if (segmentCount < 1) {
                segmentCount = 1;
            }
            precision = segmentCount;
            for (int i=0; i<=segmentCount; i++) {
                InternalEvaluate (i);
            }
        }

        public Vector3? Evaluate (float t) {
            if (t < 0 || t > 1) {
                Debug.LogError ("t must be [0, 1]");
                return null;
            }

            float targetKey = t * precision;
            int targetKeyFloor = Mathf.FloorToInt (targetKey);
            int targetKeyCeiling = Mathf.CeilToInt (targetKey);

            if (targetKey == targetKeyFloor) {
                return cachedPoints[targetKeyFloor];
            } else {
                return cachedPoints[targetKeyFloor] * ((targetKeyCeiling - targetKey) / (targetKeyCeiling - targetKeyFloor)) +
                    cachedPoints[targetKeyCeiling] * ((targetKey - targetKeyFloor) / (targetKeyCeiling - targetKeyFloor));
            }
        }

        public void AddPoint (Vector3 point) {
            points.Add (point);
        }

        public int GetPointCount () {
            return points.Count;
        }
        
        private Vector3? InternalEvaluate (int cacheIndex) {
            float t = (float)cacheIndex / precision;
            
            if (t < 0 || t > 1) {
                Debug.LogError ("t must be [0, 1]");
                return null;
            }
            
            int n = points.Count - 1;
            if (points.Count < 2) {
                Debug.LogError ("needs at least 2 points");
                return null;
            }

            Vector3 result = Vector3.zero;
            for (int i=0; i<=n; i++) {
                result += (GetCombination (n, i) * points[i] * Mathf.Pow (1f - t, n - i) * Mathf.Pow (t, i));
            }

            cachedPoints.Add (cacheIndex, result);
            return result;
        }

        public static long GetCombination (int m, int n) {
            if (m < n) {
                return 0;
            } else if (n == 0) {
                return 1;
            }
            long result = 1;
            for (int i=0; i<=n-1; i++) {
                result *= (m - i);
            }
            for (; n > 0; n--) {
                result /= n;
            }

            return result;
        }
        
    }
}