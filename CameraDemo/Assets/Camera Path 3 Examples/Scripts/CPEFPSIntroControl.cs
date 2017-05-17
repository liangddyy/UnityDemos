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

public class CPEFPSIntroControl : MonoBehaviour
{
    [SerializeField]
    private CameraPath path;
    private CameraPathAnimator animator;

    [SerializeField]
    private GameObject playerModel;

    [SerializeField]
    private Transform playerFace;

    [SerializeField]
    private GameObject playerFPSController;

    [SerializeField]
    private Camera introCamera;

    private Camera fpsCamera;

    private void Start()
    {
        animator = path.GetComponent<CameraPathAnimator>();
        fpsCamera = playerFPSController.GetComponentInChildren<Camera>();
        fpsCamera.enabled = false;
        playerModel.SetActive(true);
        Invoke("SetValuesOnFPSControllerSettled", 1.0f);
        path.orientationList[path.realNumberOfPoints - 1].rotation = fpsCamera.transform.rotation;
        playerFPSController.GetComponent<CPEMouseLook>().enabled = false;
    }

    private void SetValuesOnFPSControllerSettled()
    {
        path[path.realNumberOfPoints - 1].worldPosition = fpsCamera.transform.position;
    }

    public void StartFPS()
    {
        playerFPSController.SetActive(true);
        animator.Stop();
        animator.gameObject.SetActive(false);
        playerModel.SetActive(false);
        fpsCamera.enabled = true;
        introCamera.enabled = false;
        playerFPSController.GetComponent<CPEMouseLook>().enabled = true;
    }
}
