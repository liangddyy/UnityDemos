using UnityEngine;
using System.Collections;

public class PinchZoom : MonoBehaviour
{
    /*
    Take note that the effect of zooming in and out using this code is using the field of view and not changing the position of the camera itself.
    Your camera's projection has to be set to PERSPECTIVE from the unity inspector for the zoom function to work.
    */


    private float minPinchSpeed = 5.0f;
    private float minDistance = 5.0f;

    private float touchDelta;

    private Vector2 previousDistance;
    private Vector2 currentDistance;
    private float speedTouch0;
    private float speedTouch1;

    // change the zooming speed.
    // when using mouse, the greater this variable, the lesser you have to scroll to zoom in or out.
    // on touch devices, this will affect the sensitivity of pinching action.
    // in principle, the larger the device, the greater this variable should be.
    public float speed = 3;

    // variables to adjust the maximun and minimum camera distance.
    // the higher the number means the camera is set farther from the object.
    // the lower the number means the camera is set nearer to the object.
    public float maxOut = 40;
    public float maxIn = 15;

    // variable to show or hide the GUI scrollbar.
    public bool showGUI = true;

    void Update()
    {

        // this part of the script is for touch enabled devices (mobile phone / tablet).
        if (Input.touchCount == 2 && Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved)
        {
            currentDistance = Input.GetTouch(0).position - Input.GetTouch(1).position;
            previousDistance = ((Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition) - (Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition));
            touchDelta = currentDistance.magnitude - previousDistance.magnitude;
            speedTouch0 = Input.GetTouch(0).deltaPosition.magnitude / Input.GetTouch(0).deltaTime;
            speedTouch1 = Input.GetTouch(1).deltaPosition.magnitude / Input.GetTouch(1).deltaTime;

            if ((touchDelta + minDistance <= 5) && (speedTouch0 > minPinchSpeed) && (speedTouch1 > minPinchSpeed))
            {
                this.GetComponent<Camera>().fieldOfView = Mathf.Clamp(this.GetComponent< Camera > ().fieldOfView + (1 * speed), maxIn, maxOut);
            }

            if ((touchDelta + minDistance > 5) && (speedTouch0 > minPinchSpeed) && (speedTouch1 > minPinchSpeed))
            {
                this.GetComponent<Camera> ().fieldOfView = Mathf.Clamp(this.GetComponent< Camera > ().fieldOfView - (1 * speed), maxIn, maxOut);
            }
        }

        // this part of the script is for non-touch enabled devices (mac/windows/web).
        // the zoom effect is achieved by using mouse scroll wheel.
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            this.GetComponent< Camera > ().fieldOfView = Mathf.Clamp(this.GetComponent< Camera > ().fieldOfView + (1 * speed), maxIn, maxOut);

        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            this.GetComponent< Camera > ().fieldOfView = Mathf.Clamp(this.GetComponent< Camera > ().fieldOfView - (1 * speed), maxIn, maxOut);

        }
    }

    // simply delete these GUIs when you are no longer using them.
    void OnGUI()
    {
        if (showGUI)
        {
            // change the zooming speed.
            GUI.Label(new Rect(25, 175, 150, 50), "Zoom Speed");
            speed = GUI.HorizontalSlider(new Rect(250, 180, 100, 20), speed, 1, 5);
            GUI.Label(new Rect(200, 175, 50, 50), speed.ToString());

            // change the camera's furthest distance.
            GUI.Label(new Rect(25, 200, 150, 50), "Camera Minimum Zoom");
            maxOut = GUI.HorizontalSlider(new Rect(250, 205, 100, 20), maxOut, 40, 60);
            GUI.Label(new Rect(200, 200, 50, 50), maxOut.ToString());

            // change the camera's nearest distance.
            GUI.Label(new Rect(25, 225, 150, 50), "Camera Maximum Zoom");
            maxIn = GUI.HorizontalSlider(new Rect(250, 230, 100, 20), maxIn, 15, 30);
            GUI.Label(new Rect(200, 225, 50, 50), maxIn.ToString());
        }
    }
}