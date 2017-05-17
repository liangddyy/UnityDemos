using UnityEngine;
using System.Collections;
/// <summary>
/// 摄像机 水平运动
/// </summary>
public class HoriPivot : MonoBehaviour
{
    // variable that determine the speed of rotation
    public float rotationSpeed;

    // variable that determine the speed of lerping (deceleration)
    public float lerpSpeed;

    // variable that holds the current rotation speed
    private float speed;

    // timer to check whether the touch is a valid rotation, to prevent camera jerking on mobile device
    private float holdTimer = 0.0f;

    // variable to hold the x-axis from mouse
    private float xAxis = 0.0f;
    private int lastTouch = 0;

    void Update()
    {
        if (Input.touchCount < 2 && lastTouch < 2)
        {
            if (Input.touchCount == 0) lastTouch = 0;
            // adds up the timer everytime the user holds the left mouse button/one finger touch in mobile device
            if (Input.GetMouseButton(0)) holdTimer++;

            // if the user hold for more than 3 frame, record the mouse x-axis
            if (Input.GetMouseButton(0) && holdTimer > 3)
            {
                holdTimer++;
                xAxis = Input.GetAxis("Mouse X");
                speed = xAxis;
            }
            // else the user is not holding the mouse click anymore, begin calculating the lerp speed 
            else
            {
                var i = Time.deltaTime * lerpSpeed;
                speed = Mathf.Lerp(speed, 0, i);
            }

            // if the user release the mouse/touch, reset the timer
            if (Input.GetMouseButtonUp(0))
            {
                holdTimer = 0;
            }
            // rotate the object
            transform.Rotate(0.0f, speed * rotationSpeed, 0.0f, Space.World);
        }
        else
        {
            lastTouch = Input.touchCount;
            speed = 0;
        }
    }
}