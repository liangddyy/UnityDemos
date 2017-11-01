using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ChoseCar : MonoBehaviour {
    public static ChoseCar instance;
    //public GameObject car;
    public GameObject carRed;
    public GameObject carGreen;
    public static bool isCarRed;

    //GameObject City;

    // Use this for initialization
    void Start () {
        instance = this;
        isCarRed = true;
        //City.name = GameObject.Find("Canvas").transform.Find("Grid").transform.GetComponent<UI_Control_ScrollFlow>().Current.name;
    }

    // Update is called once per frame
    void Update () {
	    
	}
    public void isCar()
    {
        if (isCarRed)
        {
            carGreen.SetActive(true);
            carRed.SetActive(false);
            isCarRed = false;
        }
        else
        {
            carRed.SetActive(true);
            carGreen.SetActive(false);
            isCarRed = true;
        }
    }
    public void startGame()
    {
        if(GameObject.Find("Canvas").transform.Find("Grid").transform.GetComponent<UI_Control_ScrollFlow>().Current.name == "1")
        {
            PlayerPrefs.SetInt("IntCity", 1);
        }
        else
        {
            PlayerPrefs.SetInt("IntCity", 2);
        }

        SceneManager.LoadScene("Scene1");
        //SceneManager.LoadSceneAsync("Scene1");
    }
    public void exitGame()
    {
        Application.Quit();
    }

}
