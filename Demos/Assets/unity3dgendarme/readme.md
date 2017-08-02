# Unity 3D Gendarme Plugin

This is an Editor plugin that lets you easily generate Gendarme reports of your Unity project.

[Gendarme](http://www.mono-project.com/Gendarme) is a Mono utility that audits your code and gives you a report of possible issues.  This can be really useful for finding small errors, improving your coding style, and learning obscure features of C#.  But remember, you can often safely ignore Gendarme's recommendations if they seem crazy.

For OS X, Gendarme requires [Mono version 2.10](http://www.go-mono.com/mono-downloads/download.html) or higher to be installed on your system.  This won't interfere with Unity's version on Mono.  This plugin hasn't been tested on Windows yet.

![menu location](/darktable/unity3d-gendarme-plugin/downloads/gendarme-menu.png)

The "Generate Report..." option is found at the bottom of the "Assets" menu. This will open a window where you can select what level of errors you want reported, and what confidence level you want Gendarme to have before reporting the error.

After you click "Start" it will generate an HTML report that you can find in Plugins/Editor/Gendarme that will tell you about potential issues in your code.

(p.s. "Gendarme" is French for policeman.)