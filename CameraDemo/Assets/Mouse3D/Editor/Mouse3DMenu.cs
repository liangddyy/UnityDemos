using UnityEditor;
using UnityEngine;


public class Mouse3DMenu {

	[MenuItem("Window/Mouse 3D")]
	static public void AddPanel ()
	{
		EditorWindow.GetWindow<Mouse3DEditorWindow>(false, "Mouse 3D", true);		
	}
	
}
