using UnityEngine;
using System.Collections;

public class TapTutorial : MonoBehaviour {

    void OnTap(TapGesture gesture)
    {
        /* your code here */
        Debug.Log("Tap gesture detected at " + gesture.Position +". It was sent by " + gesture.Recognizer.name);
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
