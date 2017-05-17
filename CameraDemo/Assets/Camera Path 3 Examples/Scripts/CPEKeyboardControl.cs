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

public class CPEKeyboardControl : MonoBehaviour 
{

    [SerializeField]
    private CameraPathAnimator pathAnimator;

    private Camera aniamtedCam;

    [SerializeField]
    private KeyCode playBind = KeyCode.Space;
    [SerializeField]
    private KeyCode pauseBind = KeyCode.P;
    [SerializeField]
    private KeyCode stopBind = KeyCode.S;
    [SerializeField]
    private KeyCode reverseBind = KeyCode.R;
    [SerializeField]
    private KeyCode zoomInBind = KeyCode.Equals;
    [SerializeField]
    private KeyCode zoomOutBind = KeyCode.Minus;
    [SerializeField]
    private KeyCode speedUpBind = KeyCode.UpArrow;
    [SerializeField]
    private KeyCode slowDownBind = KeyCode.DownArrow;

    private void Start()
    {
        aniamtedCam = pathAnimator.animationObject.GetComponent<Camera>();
    }

    private void Update()
    {
        if (Input.GetKeyUp(playBind))
            pathAnimator.Play();

        if (Input.GetKeyUp(pauseBind))
            pathAnimator.Pause();

        if (Input.GetKeyUp(stopBind))
            pathAnimator.Stop();

        if(Input.GetKeyUp(reverseBind))
        {
            if(pathAnimator.animationMode == CameraPathAnimator.animationModes.once)
                pathAnimator.animationMode = CameraPathAnimator.animationModes.reverse;
            else
                pathAnimator.animationMode = CameraPathAnimator.animationModes.once;
        }

        if(aniamtedCam != null)
        {
            if (Input.GetKey(zoomOutBind))
                aniamtedCam.fieldOfView++;

            if (Input.GetKey(zoomInBind))
                aniamtedCam.fieldOfView--;
        }

        if (Input.GetKey(speedUpBind))
            pathAnimator.pathSpeed += Time.deltaTime;

        if (Input.GetKey(slowDownBind))
            pathAnimator.pathSpeed += -Time.deltaTime;
    }
}
