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
///This script is used in the Basic Example scene
///It is a quick C# script demonstrating the functionality you can access via scripting
///It's really quick and dirty though so don't shoot me... :o)
/// </summary>
public class ExternalControlExample : MonoBehaviour
{

    [SerializeField]
    private CameraPathAnimator pathAnimatorA;
    [SerializeField]
    private CameraPathAnimator pathAnimatorB;

    private CameraPathAnimator pathAnimator;
    private float seekTo = 0;

    void Awake()
    {
        if (pathAnimatorA == null)
            return;

        pathAnimator = pathAnimatorA;

        pathAnimatorA.playOnStart = true;
        pathAnimatorB.playOnStart = false;
    }

    void OnGUI()
    {
        if (pathAnimator == null)
            return;

        GUILayout.BeginVertical("Box", GUILayout.Width(250));

        GUILayout.BeginHorizontal();

        if (!pathAnimator.isPlaying)
        {
            if (GUILayout.Button("REPLAY", GUILayout.Width(70)))
            {
                if (pathAnimator.animationMode != CameraPathAnimator.animationModes.reverse)
                    pathAnimator.Seek(0);
                else
                    pathAnimator.Seek(1);
                pathAnimator.Play();
            }
        }
        else
        {
            if (GUILayout.Button("START", GUILayout.Width(70)))
                pathAnimator.Play();
            if (GUILayout.Button("PAUSE", GUILayout.Width(70)))
                pathAnimator.Pause();
            if (GUILayout.Button("STOP", GUILayout.Width(60)))
                pathAnimator.Stop();
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Current Animation Percentage " + (pathAnimator.percentage * 100).ToString("F1") + "%", GUILayout.Width(150));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Path: " + pathAnimator.gameObject.name);
        if (GUILayout.Button("SWITCH", GUILayout.Width(70)))
        {
            pathAnimator.Stop();
            if (pathAnimator == pathAnimatorA)
                pathAnimator = pathAnimatorB;
            else
                pathAnimator = pathAnimatorA;
            pathAnimator.Play();
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Seek to Percent " + (seekTo * 100).ToString("F1") + "%", GUILayout.Width(150));
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        seekTo = GUILayout.HorizontalSlider(seekTo, 0, 1);
        if (GUILayout.Button("Seek", GUILayout.Width(40)))
        {
            pathAnimator.Stop();
            pathAnimator.Seek(seekTo);
            pathAnimator.Play();
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

        GUILayout.BeginVertical();
        GUILayout.Label("Animation Mode:");
        GUILayout.Label(pathAnimator.animationMode.ToString());
        GUILayout.EndVertical();

        GUILayout.BeginVertical();
        GUILayout.Label("Orientation Mode:");
        GUILayout.Label(pathAnimator.orientationMode.ToString());
        GUILayout.EndVertical();

        GUILayout.EndHorizontal();


        GUILayout.BeginHorizontal();

        GUILayout.BeginVertical();
        if (GUILayout.Button("Forward"))
            pathAnimator.animationMode = CameraPathAnimator.animationModes.once;
        if (GUILayout.Button("Reverse"))
            pathAnimator.animationMode = CameraPathAnimator.animationModes.reverse;
        if (GUILayout.Button("Loop"))
            pathAnimator.animationMode = CameraPathAnimator.animationModes.loop;
        if (GUILayout.Button("Reverse Loop"))
            pathAnimator.animationMode = CameraPathAnimator.animationModes.reverseLoop;
        if (GUILayout.Button("Ping Pong"))
            pathAnimator.animationMode = CameraPathAnimator.animationModes.pingPong;
        GUILayout.EndVertical();

        GUILayout.BeginVertical();
        if (GUILayout.Button("Custom"))
            pathAnimator.orientationMode = CameraPathAnimator.orientationModes.custom;
        if (GUILayout.Button("Mouse look"))
            pathAnimator.orientationMode = CameraPathAnimator.orientationModes.mouselook;
        if (GUILayout.Button("Follow Path"))
            pathAnimator.orientationMode = CameraPathAnimator.orientationModes.followpath;
        if (GUILayout.Button("Reverse Follor Path"))
            pathAnimator.orientationMode = CameraPathAnimator.orientationModes.reverseFollowpath;
        if (GUILayout.Button("CameraPathOnRailsTarget"))
            pathAnimator.orientationMode = CameraPathAnimator.orientationModes.target;
        if (GUILayout.Button("Follow Transform"))
            pathAnimator.orientationMode = CameraPathAnimator.orientationModes.followTransform;
        GUILayout.EndVertical();

        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
    }
}