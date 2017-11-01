using UnityEngine;
using System.Collections;

public class testrefsh : MonoBehaviour {
    public UI_Control_ScrollFlow _ScrollFlow;
	// Use this for initialization
	void Start () {
        _ScrollFlow.Refresh();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.V))
        {
            print(transform.GetComponent<UI_Control_ScrollFlow>().Current.name);//2 (UI_Control_ScrollFlow_Item)
        }
	}
}
