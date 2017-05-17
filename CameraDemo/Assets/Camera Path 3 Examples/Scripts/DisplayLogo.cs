// Camera Path 3
// Available on the Unity Asset Store
// Copyright (c) 2013 Jasper Stocker http://support.jasperstocker.com/camera-path/
// For support contact email@jasperstocker.com
//
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY 
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.

using UnityEngine;

/// <summary>
/// Just displays the camera path logo in the bottom left corner
/// </summary>
public class DisplayLogo : MonoBehaviour
{
    [SerializeField]
    private Texture2D logo;

    private const int WIDTH = 100;
    private const int HEIGHT = 80;

    void OnGUI()
    {
        GUI.DrawTexture(new Rect(5, Screen.height - HEIGHT - 5, WIDTH, HEIGHT), logo);
    }
}