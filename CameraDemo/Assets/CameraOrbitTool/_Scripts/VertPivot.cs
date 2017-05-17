using UnityEngine;
using System.Collections;

public class VertPivot : MonoBehaviour
{
    // Define the limit of camera position as well as rotation
    public float cameraTopLimit;
    public float cameraBottomLimit;

    // Position period that the camera won't rotate
    public float thresholdTop;
    public float thresholdBottom;

    // variables to define the camera movement speed, camera rotation speed, and deceleration(lerp) speed respectively
    public float moveSpeed;
    public float rotationSpeed;
    public float lerpSpeed;

    // GUI variable; true = show GUI, false = hide GUI
    // Alternatively, you can delete this variable and everything inside function OnGUI () to remove the GUI altogether
    public bool showGUI = true;

    // hold timer to prevent the camera from jerking in mobile device
    private float holdTimer = 0f;

    // holds the y-axis retrieved from mouse
    private float yaxis;

    // holds the current 
    private float speed;

    public float minimumTilt = -30F;
    public float maximumTilt = 30F;
    private float rotationY = 0F;
    private Quaternion originalRotation;
    private float originalXRot;
    private float originalYRot;
    private int lastTouch = 0;

    void Start()
    {
        originalRotation = transform.localRotation;
        originalXRot = transform.localEulerAngles.x;
        originalYRot = transform.localEulerAngles.y;
    }


    void Update()
    {
        if (Input.touchCount < 2 && lastTouch < 2)
        {
            if (Input.touchCount == 0) lastTouch = 0;
            // adds up the timer everytime the user holds the left mouse button/one finger touch in mobile device
            if (Input.GetMouseButton(0)) holdTimer++;

            // if the user hold for more than 3 frame, record the mouse y-axis
            if (Input.GetMouseButton(0) && holdTimer > 3)
            {
                // reverse the received y-axis to create inverse-axis movement (remove minus sign if you want normal-axis movement)
                yaxis = -Input.GetAxis("Mouse Y");
                speed = yaxis;
            }
            // else the user is not holding the mouse click anymore, begin calculating the lerp speed 
            else
            {
                var ix = Time.deltaTime * lerpSpeed;
                speed = Mathf.Lerp(speed, 0, ix);
            }

            // if the user release the mouse/touch, reset the timer
            if (Input.GetMouseButtonUp(0))
            {
                holdTimer = 0;
            }

            // calculate the movement of the camera, clamp it so that it won't exceed the limit
            float limitY = Mathf.Clamp(transform.position.y + (speed * moveSpeed), cameraBottomLimit, cameraTopLimit);
            //transform.position.y= limitY;
            transform.position = new Vector3(transform.position.x, limitY, transform.position.z);
            
            // if the camera pos still inside the limit, rotate the camera as well
            if (!(transform.position.y < thresholdTop && transform.position.y > thresholdBottom))
            {
                rotationY += speed * -rotationSpeed * 0.8f;
                if (transform.position.y > thresholdTop)
                {
                    rotationY = ClampAngle(rotationY, -maximumTilt, 1F);
                }
                else if (transform.position.y < thresholdBottom)
                {
                    rotationY = ClampAngle(rotationY, 1F, -minimumTilt);
                }
                Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, Vector3.left);
                transform.localRotation = originalRotation * yQuaternion;
            }
        }
        else
        {
            lastTouch = Input.touchCount;
            speed = 0;
        }
    }

    // function to clamp the angle based on given min and max
    static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }

    // these GUI-s are for sample purpose
    // simply delete line 97 - 135 to remove these GUI-s
    void OnGUI()
    {
        if (showGUI)
        {
            // change the limit of camera movement top
            GUI.Label(new Rect(25, 25, 150, 50), "Camera Top Limit");
            cameraTopLimit = GUI.HorizontalSlider(new Rect(250, 30, 100, 20), cameraTopLimit, 50, 150);
            GUI.Label(new Rect(200, 25, 50, 50), cameraTopLimit.ToString("F2"));

            // change the limit of camera movement bottom
            GUI.Label(new Rect(25, 50, 150, 50), "Camera Bot Limit");
            cameraBottomLimit = GUI.HorizontalSlider(new Rect(250, 55, 100, 20), cameraBottomLimit, 0, -100);
            GUI.Label(new Rect(200, 50, 50, 50), cameraBottomLimit.ToString("F2"));

            // change the limit of when the camera will start rotating top
            GUI.Label(new Rect(25, 75, 150, 50), "Top Rotation Threshold");
            thresholdTop = GUI.HorizontalSlider(new Rect(250, 80, 100, 20), thresholdTop, 25, 75);
            GUI.Label(new Rect(200, 75, 50, 50), thresholdTop.ToString("F2"));

            // cahnge the limit of when the camera will start rotating bottom
            GUI.Label(new Rect(25, 100, 150, 50), "Bot Rotation Threshold");
            thresholdBottom = GUI.HorizontalSlider(new Rect(250, 105, 100, 20), thresholdBottom, -25, 25);
            GUI.Label(new Rect(200, 100, 50, 50), thresholdBottom.ToString("F2"));

            // change the camera movement speed
            GUI.Label(new Rect(25, 125, 150, 50), "Camera Movement Speed");
            moveSpeed = GUI.HorizontalSlider(new Rect(250, 130, 100, 20), moveSpeed, 0, 2);
            GUI.Label(new Rect(200, 125, 50, 50), moveSpeed.ToString("F2"));

            // change the camera vertical rotation speed
            GUI.Label(new Rect(25, 150, 150, 50), "Camera Rotation Speed");
            rotationSpeed = GUI.HorizontalSlider(new Rect(250, 155, 100, 20), rotationSpeed, 0, 2);
            GUI.Label(new Rect(200, 150, 50, 50), rotationSpeed.ToString("F2"));

            // to change to sphere orbit scene
            if (GUI.Button(new Rect(Screen.width - 100, 25, 75, 50), "Sphere\nOrbit"))
            {
                Application.LoadLevel(1);
            }

        }
    }
}