using UnityEngine;
using System.Collections;

using LDFW.Math;

public class BezierDemo : MonoBehaviour {

    private BezierCurve curve;
    public AnimationCurve curve2;

	void Start () {
        curve = new BezierCurve ();

        curve.AddPoint (new Vector3 (0, 0, 0));
        curve.AddPoint (new Vector3 (1, 1, 1));
        curve.AddPoint (new Vector3 (5, 1, 1));
        curve.AddPoint (new Vector3 (3, 5, 1));
        curve.AddPoint (new Vector3 (-1.8f, 5, 1));
        curve.AddPoint (new Vector3 (-5.3f, 3.3f, 1));
        curve.AddPoint (new Vector3 (-6.8f, 0.24f, 1));
        curve.GenerateCache (1);

        for (float i=0; i<=30f; i++) {
            GameObject temp = new GameObject ();
            temp.transform.SetParent (transform);
            temp.transform.position = (Vector3) curve.Evaluate (i / 30f);
            temp.name = "temp" + i;
        }

	}
    

	
}
