using UnityEngine;
using System.Collections;

public class CameraFollowScript : MonoBehaviour {

    public GameObject city1;
    public GameObject city2;
    public GameObject carRed;
    public GameObject carGreen;
    private GameObject car;
    public float distance = 5;
    public float heightDiff = 3;

    public float angleDamping = 1.5f; //缓冲系数
    public float heightDamping = 1f;

    //GameObject City;
    int intCity;

    void Start()
    {
        if (ChoseCar.isCarRed)
        {
            carGreen.SetActive(false);
            carRed.SetActive(true);
            car = carRed;
        }
        else
        {
            carRed.SetActive(false);
            carGreen.SetActive(true);
            car = carGreen;
        }

        //City.name = GameObject.Find("").transform.Find("").transform.GetComponent<UI_Control_ScrollFlow>().Current.name;
        intCity = PlayerPrefs.GetInt("IntCity");
        if(intCity == 1)
        {
            city1.SetActive(true);
            city2.SetActive(false);
        }
        else
        {
            city1.SetActive(false);
            city2.SetActive(true);
        }
    }

    void Update()
    {
        float myH = transform.position.y;
        float dstH = car.transform.position.y + heightDiff;
        float retH = Mathf.Lerp(myH, dstH, heightDamping * Time.deltaTime);

        float myAngle = transform.eulerAngles.y;
        float dstAngle = car.transform.eulerAngles.y;
        float retAngle = Mathf.LerpAngle(myAngle, dstAngle, angleDamping * Time.deltaTime);
        Quaternion retRotation = Quaternion.Euler(0, retAngle, 0);

        transform.position = car.transform.position;
        transform.position -= retRotation * Vector3.forward * distance;
        Vector3 temp = transform.position;
        temp.y = retH;
        transform.position = temp;

        transform.LookAt(car.transform);
    }

}
