using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void TestScence(string str)
    {
        Debug.Log("test");
        SceneManager.LoadScene(str);
    }
}
