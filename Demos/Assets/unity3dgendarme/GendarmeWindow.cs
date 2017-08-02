//	Copyright (c) 2012 Calvin Rien
//        http://the.darktable.com
//	
//	This software is provided 'as-is', without any express or implied warranty. In
//	no event will the authors be held liable for any damages arising from the use
//	of this software.
//	
//	Permission is granted to anyone to use this software for any purpose,
//	including commercial applications, and to alter it and redistribute it freely,
//	subject to the following restrictions:
//	
//	1. The origin of this software must not be misrepresented; you must not claim
//	that you wrote the original software. If you use this software in a product,
//	an acknowledgment in the product documentation would be appreciated but is not
//	required.
//	
//	2. Altered source versions must be plainly marked as such, and must not be
//	misrepresented as being the original software.
//	
//	3. This notice may not be removed or altered from any source distribution.

using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using DB = UnityEngine.Debug;
using System.Text.RegularExpressions;
using System;

public class GendarmeWindow : EditorWindow {

	sealed internal class GendarmeParam {
		public static readonly string[] modifiers = {"and higher","only","and lower"};

		public readonly string[] levels;

		public int Index{
			get;
			set;
		}

		public int Modifier{
			get;
			set;
		}

		public GendarmeParam(params string[] lvls) {
			Index = 0;
			Modifier = 0;
			levels = lvls;
		}

		public override string ToString() {
			var mod = "";

			if (Index != 0) {
				if (Modifier == 0) {
					mod = "+";
				}
				else if (Modifier == 2) {
					mod = "-";
				}
			}
	
			return string.Format("{0}{1}", levels[Index].ToLower(), mod);
		}
	}


	GendarmeParam severity = new GendarmeParam("All","Critical","High","Medium","Low");
	GendarmeParam confidence = new GendarmeParam("All","Total","High","Normal","Low");


	Action guiAction;

	Dictionary<string,string> result;

	static void ShowDialog(string title, Action content) {
		EditorGUILayout.BeginVertical();

		GUILayout.Label (title, EditorStyles.boldLabel);

		GUILayout.Space(16);

		EditorGUILayout.BeginHorizontal();

		if (content != null)
			content();

		EditorGUILayout.EndHorizontal();

		EditorGUILayout.EndVertical();
	}

	void StartGUI() {
		ShowDialog("Gendarme Report Level", ()=>{
			GUILayout.FlexibleSpace();

			var severeLabel = new GUIContent("Severity", "How serious the issues discovered are.");


			GUILayout.Label(severeLabel);

			severity.Index = EditorGUILayout.Popup(severity.Index, severity.levels);

			if (severity.Index == 0)
				GUI.enabled = false;

			severity.Modifier = EditorGUILayout.Popup(severity.Modifier, GendarmeParam.modifiers);

			GUI.enabled = true;
	
			EditorGUILayout.EndHorizontal();
	
			GUILayout.Space(16);
	
			EditorGUILayout.BeginHorizontal();

			var confidLabel = new GUIContent("Confidence", "How certain Gendarme is that your code has this issue.");

			GUILayout.Label(confidLabel);

			confidence.Index = EditorGUILayout.Popup(confidence.Index, confidence.levels);

			if (confidence.Index == 0)
				GUI.enabled = false;

			confidence.Modifier = EditorGUILayout.Popup(confidence.Modifier, GendarmeParam.modifiers);

			GUI.enabled = true;

			EditorGUILayout.EndHorizontal();

			GUILayout.Space(16);

			EditorGUILayout.BeginHorizontal();
	
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Start", GUILayout.MinWidth(150))) {
				guiAction = WorkingGUI;
			}
		});
	}

	void ResultsGUI() {
		ShowDialog("Gendarme Report", ()=>{
			GUILayout.Label ("Report is ready for viewing.");
	
			EditorGUILayout.EndHorizontal();
	
			GUILayout.Space(16);
	
			EditorGUILayout.BeginHorizontal();
	
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Open Report")) {
				Application.OpenURL(string.Format("file://{0}", result["report"].Replace(" ", "%20")));

				guiAction = null;
				this.Close();
				return;
			}
			if (GUILayout.Button("Done")) {
				guiAction = null;
				this.Close();
				return;
			}
		});
	}

	void ErrorGUI() {
		ShowDialog("An error as occured:", ()=>{
			GUI.enabled = false;
	
			var errorMessage = result["stderr"].Replace(", ", ",\n").Replace(": ", ":\n").Replace(". ", ".\n");
	
			EditorGUILayout.TextArea(errorMessage);
	
			GUI.enabled = true;
		});
	}

	static void WorkingGUI() {
		ShowDialog("Working...", null);
	}


	static void SetupGUI() {
		ShowDialog("Setting up...", null);
	}

	void OnGUI () {
		if (guiAction == null) {
			result = new Dictionary<string, string>();

			if (GendarmeRunner.SetupDone()) {
				guiAction = StartGUI;
			}
			else {
				guiAction = SetupGUI;
			}
		}

		guiAction();
	}

	void OnInspectorUpdate() {
		if (guiAction == SetupGUI) {
			GendarmeRunner.Setup();

			guiAction = StartGUI;

			Repaint();
		}
		else if (guiAction == WorkingGUI) {
			result = GendarmeRunner.Run(severity.ToString(), confidence.ToString());

			if (!string.IsNullOrEmpty(result["stderr"])) {
				guiAction = ErrorGUI;
			}
			else {
				guiAction = ResultsGUI;
			}

			Repaint();
		}
    }
}
