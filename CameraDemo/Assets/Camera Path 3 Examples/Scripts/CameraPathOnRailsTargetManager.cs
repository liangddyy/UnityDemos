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
/// This is a script used in the On Rails Shooter example
/// It controls when targets are shown by listening to custom events specified in the camera path
/// </summary>
public class CameraPathOnRailsTargetManager : MonoBehaviour 
{
    [SerializeField]
    private CameraPathAnimator animator;

    [SerializeField]
    private CameraPathOnRailsTarget[] Scene1Targets;
    [SerializeField]
    private CameraPathOnRailsTarget[] Scene2Targets;
    [SerializeField]
    private CameraPathOnRailsTarget[] Scene3Targets;
    [SerializeField]
    private CameraPathOnRailsTarget[] Scene4Targets;

    private bool showGameOver = false;

    void Awake()
    {

        if (animator == null)
        {
            Debug.LogError("No animator assgined the the event listener");
            Destroy(gameObject);
            return;
        }

        animator.AnimationCustomEvent += OnCustomEvent;
        animator.AnimationFinishedEvent += OnFinish;
    }

    void OnGUI()
    {
        if(showGameOver)
        {   
            if(GUILayout.Button("Replay"))
            {
                Application.LoadLevel(Application.loadedLevel);
            }
        }
    }

    private void OnFinish()
    {
        showGameOver = true;
    }

    private void OnCustomEvent(string eventname)
    {
        switch(eventname)
        {
            case "Scene1":
                foreach (CameraPathOnRailsTarget target in Scene1Targets)
                {
                    target.Show();
                }
                break;
            case "Scene1End":
                foreach (CameraPathOnRailsTarget target in Scene1Targets)
                {
                    target.Hide();
                }
                break;
            case "Scene2":
                foreach (CameraPathOnRailsTarget target in Scene2Targets)
                {
                    target.Show();
                }
                break;
            case "Scene2End":
                foreach (CameraPathOnRailsTarget target in Scene2Targets)
                {
                    target.Hide();
                }
                break;
            case "Scene3":
                foreach (CameraPathOnRailsTarget target in Scene3Targets)
                {
                    target.Show();
                }
                break;
            case "Scene3End":
                foreach (CameraPathOnRailsTarget target in Scene3Targets)
                {
                    target.Hide();
                }
                break;
            case "Scene4":
                foreach (CameraPathOnRailsTarget target in Scene4Targets)
                {
                    target.Show();
                }
                break;
            case "Scene4End":
                foreach (CameraPathOnRailsTarget target in Scene4Targets)
                {
                    target.Hide();
                }
                break;
        }
    }
}
