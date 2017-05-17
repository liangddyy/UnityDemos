using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace LDFW.Path {

    public class LineRenderer : MonoBehaviour {

        // Public variables
        //public Transform linePathPointsParent;
        public Transform[] pathPoints;
        public float lineWidth = 1f;
        public int lineSegmentCount = 10;
        public AnimationCurve xAnimationCurve;
        public AnimationCurve yAnimationCurve;
        public AnimationCurve zAnimationCurve;
        public Transform referenceCamera;

        // Private variables
        MeshFilter meshFilter;
        MeshRenderer meshRenderer;
        Mesh mesh;
        List<Vector3> vertices = new List<Vector3> ();
        List<Vector2> uvs = new List<Vector2> ();
        List<int> triangles = new List<int> ();

        void Awake () {
            
            meshFilter = GetComponent<MeshFilter> ();
            meshRenderer = GetComponent<MeshRenderer> ();

            if (meshFilter == null) {
                meshFilter = gameObject.AddComponent<MeshFilter> ();
            }
            if (meshRenderer == null) {
                meshRenderer = gameObject.AddComponent<MeshRenderer> ();
            }

            mesh = new Mesh ();
            meshFilter.mesh = mesh;
        }

        // Constant Z
        [ContextMenu ("InitWithRespectToXYPlane")]
        public void InitWithRespectToXYPlane () {
            //if (linePathPointsParent == null) {

            transform.position = Vector3.zero;

            if (pathPoints == null) {
                Debug.LogError ("path is null!");
                return;
            }

            if (pathPoints.Length < 2) {
                Debug.LogError ("path needs at least two points");
                return;
            }


            // Get the camera position;
            Vector3 cameraPosition = Vector3.zero;
            if (referenceCamera == null) {
                cameraPosition = Camera.main.transform.position;
            } else {
                cameraPosition = referenceCamera.position;
            }

            // Calculate total segmented straight line lenth
            float totalLength = 0;
            for (int i=1; i<pathPoints.Length; i++) {
                totalLength += (pathPoints[i].position - pathPoints[i-1].position).magnitude;
            }
            //Debug.Log ("totalLength = " + totalLength);

            // Construct the X,Y,Z animation curves
            xAnimationCurve = new AnimationCurve ();
            yAnimationCurve = new AnimationCurve ();
            zAnimationCurve = new AnimationCurve ();
            Vector3 currentPosition = Vector3.zero;
            float lineSegmentLength = 0f;
            for (int i=0; i< pathPoints.Length; i++) {
                currentPosition = pathPoints[i].position;
                if (i == 0) {
                    xAnimationCurve.AddKey (0, currentPosition.x);
                    yAnimationCurve.AddKey (0, currentPosition.y);
                    zAnimationCurve.AddKey (0, currentPosition.z);
                } else {
                    lineSegmentLength += (currentPosition - pathPoints[i-1].position).magnitude;
                    xAnimationCurve.AddKey (lineSegmentLength / totalLength, currentPosition.x);
                    yAnimationCurve.AddKey (lineSegmentLength / totalLength, currentPosition.y);
                    zAnimationCurve.AddKey (lineSegmentLength / totalLength, currentPosition.z);
                }
            }

            // Construct vertices and UVs
            vertices = new List<Vector3> ();
            uvs = new List<Vector2> ();
            triangles = new List<int> ();

            float startSegmentTime;
            float endSegmentTime;
            float centerSegmentTime;
            Vector3 perpendicualrVectorLHS;
            Vector3 perpendicularVectorRHS;
            Vector3 perpendicularRelativePositionLHS;
            Vector3 perpendicularRelativePositionRHS;
            Vector3 currentLineSegmentStartPosition;
            Vector3 currentLineSegmentEndPosition;
            Vector3 currentLineSegmentCenterPosition;

            Vector3 cameraOffsetVector = Vector3.zero;
            if ((cameraPosition - transform.position).z > 0) {
                cameraOffsetVector.z = 1f;
            } else {
                cameraOffsetVector.z = -1f;
            }

            for (int i = 0; i <= lineSegmentCount; i++) {
                if (i == 0 ) {
                    startSegmentTime = (float)i / lineSegmentCount;
                    endSegmentTime = (float)(i + 1) / lineSegmentCount;

                } else if (i == lineSegmentCount) {
                    startSegmentTime = (float)(i - 1) / lineSegmentCount;
                    endSegmentTime = (float)i / lineSegmentCount;

                } else {
                    startSegmentTime = (float)(i - 1) / lineSegmentCount;
                    endSegmentTime = (float)(i + 1) / lineSegmentCount;

                }

                centerSegmentTime = (float)i / lineSegmentCount;


                currentLineSegmentStartPosition = new Vector3 (xAnimationCurve.Evaluate (startSegmentTime), yAnimationCurve.Evaluate (startSegmentTime), zAnimationCurve.Evaluate (startSegmentTime));
                currentLineSegmentEndPosition = new Vector3 (xAnimationCurve.Evaluate (endSegmentTime), yAnimationCurve.Evaluate (endSegmentTime), zAnimationCurve.Evaluate (endSegmentTime));
                currentLineSegmentCenterPosition = new Vector3 (xAnimationCurve.Evaluate (centerSegmentTime), yAnimationCurve.Evaluate (centerSegmentTime), zAnimationCurve.Evaluate (centerSegmentTime));

                perpendicualrVectorLHS = Vector3.Cross (currentLineSegmentEndPosition - currentLineSegmentStartPosition, cameraOffsetVector);
                perpendicularVectorRHS = Vector3.Cross (cameraOffsetVector, currentLineSegmentEndPosition - currentLineSegmentStartPosition);

                perpendicularRelativePositionLHS = currentLineSegmentCenterPosition + perpendicualrVectorLHS.normalized * lineWidth / 2f;
                perpendicularRelativePositionRHS = currentLineSegmentCenterPosition + perpendicularVectorRHS.normalized * lineWidth / 2f;


                /*
                GameObject temp = new GameObject ();
                temp.transform.parent = transform;
                temp.transform.position = perpendicularRelativePositionLHS;

                temp = new GameObject ();
                temp.transform.parent = transform;
                temp.transform.position = perpendicularRelativePositionRHS;
                */

                vertices.Add (perpendicularRelativePositionLHS);
                vertices.Add (perpendicularRelativePositionRHS);

                // calculate UV
                if (i == 0) {
                    uvs.Add (new Vector2 (0, 0));
                    uvs.Add (new Vector2 (0, 1));
                } else if (i == lineSegmentCount) {
                    uvs.Add (new Vector2 (1, 0));
                    uvs.Add (new Vector2 (1, 1));
                } else {
                    uvs.Add (new Vector2 ((float)i / lineSegmentCount, 0));
                    uvs.Add (new Vector2 ((float)i / lineSegmentCount, 1));
                }

                // calculate triangle
                if (i < lineSegmentCount) {
                    triangles.Add (i * 2);
                    triangles.Add (i * 2 + 2);
                    triangles.Add (i * 2 + 1);
                    triangles.Add (i * 2 + 1);
                    triangles.Add (i * 2 + 2);
                    triangles.Add (i * 2 + 3);
                }
            }

            mesh.vertices = vertices.ToArray ();
            mesh.uv = uvs.ToArray ();
            //mesh.triangles = triangles.ToArray ();
        }

        public void DisplayLine (float percentage) {
            int arrayLength = (int) (percentage * triangles.Count);
            while (arrayLength % 3 != 0) {
                arrayLength++;
            }

            arrayLength = Mathf.Min (arrayLength, triangles.Count);
            List<int> newTriangles = new List<int> ();
            for (int i=0; i<arrayLength; i++) {
                newTriangles.Add (triangles[i]);
            }
            mesh.triangles = newTriangles.ToArray ();
        }

        [ContextMenu ("PlayTest")]
        public void Test () {
            StartCoroutine (TestCoroutine ());
        }
        private IEnumerator TestCoroutine () {
            for (int i=0; i<200; i++) {
                DisplayLine (i * 0.01f);
                //yield return new WaitForSeconds (0.1f);
                yield return null;
            }
        }
    }

}