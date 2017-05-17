using UnityEngine;
using System.Collections;

public class AndroidTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void BtnShwMessage()
    {
        //AndroidJavaClass：通过指定类名可以构造出一个类
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

        // UnityPlayer这个类可以获取当前的Activity
        // currentActivity字符串对应源码中UnityPlayer类下 的 Activity 变量名。
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");

        // 在对象上调用一个Java方法
        jo.Call("StartAty", jo);
    }

}
