#region

using UnityEngine;

#endregion

public class VertPivot_Sphere : MonoBehaviour
{
    // hold timer to prevent the camera from jerking in mobile device
    private float holdTimer;
    private float horiSpeed;

    // this variable mainly affects the duration of deceleration
    // lesser value will lenghten the duration of deceleration
    public float lerpSpeed;
    public float maximumTilt = 30F;

    // these two variables determine the furthest tilt that the orbit will do
    // they are relative to 0 value, so the more extreme these variables are, the higher (or lower) the tilt
    public float minimumTilt = -30F;
    private Quaternion originalRotation;
    private float originalXRot;
    private float originalYRot;

    // variables to define the camera movement speed, camera rotation speed
    // adjust this variable to increase or decrease the orbiting speed
    public float rotationSpeed;

    // default variables
    private float rotationY;

    // contains the speed of every frame
    private float vertSpeed;
    private float xAxis;

    // holds the x-axis & y-axis retrieved from mouse
    private float yaxis;


    private void Start()
    {
        originalRotation = transform.localRotation;
        originalXRot = transform.localEulerAngles.x;
        originalYRot = transform.localEulerAngles.y;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0)) holdTimer++;

        // if the user hold for more than 3 frame, record the mouse y-axis
        if (Input.GetMouseButton(0) && (holdTimer > 3))
        {
            // reverse the received y-axis to create inverse-axis movement (remove minus sign if you want normal-axis movement)
            yaxis = -Input.GetAxis("Mouse Y");
            vertSpeed = yaxis;
            xAxis = Input.GetAxis("Mouse X");
            horiSpeed = xAxis;
        }
        // else the user is not holding the mouse click anymore, begin calculating the lerp speed 
        else
        {
            var ix = Time.deltaTime*lerpSpeed;
            vertSpeed = Mathf.Lerp(vertSpeed, 0, ix);
            horiSpeed = Mathf.Lerp(horiSpeed, 0, ix);
        }

        // if the user release the mouse/touch, reset the timer
        if (Input.GetMouseButtonUp(0))
            holdTimer = 0;

        rotationY += vertSpeed*-rotationSpeed*0.8f;
        rotationY = ClampAngle(rotationY, -maximumTilt, -minimumTilt);
        Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, Vector3.left);
        transform.localRotation = originalRotation*yQuaternion;

        transform.Rotate(0.0f, horiSpeed*rotationSpeed, 0.0f, Space.World);
    }

    // to clamp the angles so that the rotation will not behave erraticly
    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }

    private void OnGUI()
    {
        // to change to the pan orbit scene
        if (GUI.Button(new Rect(Screen.width - 100, 25, 75, 50), "Pan\nOrbit"))
            Application.LoadLevel(0);
    }
}