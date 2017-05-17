using UnityEngine;
using System.Collections;
/// <summary>
/// 处理多个物体的事件
/// </summary>
public class ClickTest : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
	    EventTriggerListener.Get(gameObject).onClick = Test;
	    //        foreach (var obj in CupObjs)
	    //        {
	    //            EventTriggerListener.Get(obj).onDown = Test;
	    //            EventTriggerListener.Get(obj).onClick = Test;
	    //        }
	}

    
    void Test(GameObject obj)
    {
        print("点击了" + obj.name);

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
