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

public class GendarmeRunner : ScriptableObject {
	const string monoError = "You need mono 2.10 or higher installed on your system.";

	[MenuItem("Assets/Generate Gendarme Report...")]
	static void Init() {

		EditorWindow.FocusWindowIfItsOpen(typeof(GendarmeWindow));

		var window = EditorWindow.GetWindow(typeof(GendarmeWindow), true, "Gendarme");
		window.Show();
	}

	static string JoinPath(params string[] path) {
		string output = path[0];
		
		for (int i=1; i< path.Length; i++) {
			output = Path.Combine(output, path[i]);
		}
		
		return output;
	}

	static bool ValidateMono(string path) {
		using (var process = new System.Diagnostics.Process()) {
			var info = process.StartInfo;

			info.FileName = path;
			info.Arguments = "-V";

			info.UseShellExecute = false;
			info.RedirectStandardOutput = true;

			process.Start();

			string stdOut = process.StandardOutput.ReadLine();

			process.WaitForExit();

			if (string.IsNullOrEmpty(stdOut)) {
				Debug.LogError(monoError);
				return false;
			}

			// version string looks like: "Mono JIT compiler version 2.10.8 (tarball Mon Dec 19 17:43:18 EST 2011)"
			var m = new Regex(".* ([0-9]+)\\.([0-9]+)\\.[0-9]+ .*").Match(stdOut);

			var grps = m.Groups;

			if (!m.Success) {
				Debug.LogError(monoError);

				return false;
			}

			int[] version = {0,0,0};

			for (int i=1; i < 3; i++) {
				version[i] = Convert.ToInt32(grps[i].Value);
			}

			if (version[1] < 2 || version[2] < 10) {
				Debug.LogError(monoError);

				return false;
			}
		}

		return true;
	}

	static string FindMono() {
		string monoExe = null;

		using (var process = new System.Diagnostics.Process()) {
			var info = process.StartInfo;

			info.FileName = "which";
			info.Arguments = "mono";

			info.UseShellExecute = false;
			info.RedirectStandardOutput = true;

			process.Start();

			string stdOut = process.StandardOutput.ReadLine();

			process.WaitForExit();

			if (string.IsNullOrEmpty(stdOut)) {
				Debug.LogError(monoError);
				return null;
			}

			monoExe = stdOut;
		}

		return monoExe;
	}

	public static bool SetupDone() {
		string pluginPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(ScriptableObject.CreateInstance<GendarmeRunner>())));

		string initialPath = JoinPath(pluginPath, "gendarme");

		if (!Directory.Exists(initialPath)) {
			return true;
		}

		return false;
	}

	public static void Setup() {
		string projectPath = Path.GetDirectoryName(Application.dataPath);

		string pluginPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(ScriptableObject.CreateInstance<GendarmeRunner>())));
		pluginPath = JoinPath(projectPath, pluginPath);

		string source = JoinPath(pluginPath, "gendarme");
		string dest = JoinPath(projectPath, "gendarme");

		FileUtil.MoveFileOrDirectory(source, dest);

		FileUtil.DeleteFileOrDirectory(source + ".meta");

		string[] dlls = Directory.GetFiles(dest, "*.xll", SearchOption.AllDirectories);

		foreach (var f in dlls) {
			var destName = f.Replace(".xll", ".dll");

			File.Move(f, destName);
		}


		AssetDatabase.Refresh();
	}

	static string Quote(string path) {
		return string.Format("\"{0}\"", path);
	}
	
	static List<string> GetArgs(string projectPath) {
		string copPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(ScriptableObject.CreateInstance<GendarmeRunner>())));
		copPath = JoinPath(projectPath, copPath);
		string rulesPath = JoinPath(copPath, "unity-rules.xml");
		string ignorePath = JoinPath(copPath, "unity.ignore");
		
		return new List<string>(new string[] {"--quiet", "--config", Quote(rulesPath), "--ignore", Quote(ignorePath)});
	}
	
	public static Dictionary<string,string> Run(string severity, string confidence) {
		var windows = Application.platform == RuntimePlatform.WindowsEditor;
		var result = new Dictionary<string, string>();

		string monoExe = windows ? "" : FindMono();

		if (!windows && !ValidateMono(monoExe)) {
			result["stderr"] = monoError;

			return result;
		}

		string projectPath = Path.GetDirectoryName(Application.dataPath);
		string gendarmeExe = JoinPath(projectPath, "gendarme", "gendarme.exe");
		string reportPath = JoinPath(projectPath, "gendarme-report.html");

		List<string> assemblies = new List<string>();
		foreach (var dll in Directory.GetFiles(JoinPath(projectPath, "Library", "ScriptAssemblies"), "*.dll", SearchOption.AllDirectories)) {
			assemblies.Add(Quote(dll));
		}

		using (var process = new System.Diagnostics.Process()) {

			List<string> args = GetArgs(projectPath);
			
			if (!windows) {
				args.Insert(0, Quote(gendarmeExe));
			}
			
			args.AddRange(new string[] {"--html", Quote(reportPath)});
						
			if (!string.IsNullOrEmpty(severity)) {
				args.AddRange(new string[] {"--severity", severity});
			}

			if (!string.IsNullOrEmpty(confidence)) {
				args.AddRange(new string[] {"--confidence", severity});
			}
						
			args.AddRange(assemblies);

			var info = process.StartInfo;
			
			info.FileName = windows ? gendarmeExe : monoExe;
			info.Arguments = string.Join(" ", args.ToArray()).Trim();

			info.UseShellExecute = false;
			info.RedirectStandardError = true;
			info.RedirectStandardOutput = true;
			
			process.Start();
			
			result["stdout"] = process.StandardOutput.ReadToEnd();
			result["stderr"] = process.StandardError.ReadToEnd();

			process.WaitForExit();
			
			if (!string.IsNullOrEmpty(result["stdout"])) {
				Debug.Log(result["stdout"]);
			}

			if (!string.IsNullOrEmpty(result["stderr"])) {
				Debug.LogError(result["stderr"]);
				return result;
			}

			result["report"] = reportPath;

			return result;
		}
	}
}
