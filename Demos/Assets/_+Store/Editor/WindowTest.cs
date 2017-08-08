using UnityEngine;
using System.Collections;
using UnityEditor;

public class WindowTest : EditorWindow {

[MenuItem ("GameObject/window")]
	static void AddWindow(){
		Rect  rect = new Rect (0,0,500,500);
        WindowTest window = (WindowTest)EditorWindow.GetWindowWithRect(typeof(WindowTest), rect, true, "测试窗口");
        window.Show();
	}
}
