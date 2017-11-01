using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ReturnOrExit : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ReturnGame()
    {
        SceneManager.LoadScene("Scene0");
    }
    public void exitGame()
    {
        Application.Quit();
    }
}
