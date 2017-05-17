#pragma strict

// hold timer to prevent the camera from jerking in mobile device
private var holdTimer:float = 0f;

// holds the x-axis & y-axis retrieved from mouse
private var yaxis:float;
private var xAxis:float;

// contains the speed of every frame
private var vertSpeed:float;
private var horiSpeed:float;

// default variables
private var rotationY:float = 0F;
private var originalRotation:Quaternion;
private var originalXRot:float;
private var originalYRot:float;

// variables to define the camera movement speed, camera rotation speed
// adjust this variable to increase or decrease the orbiting speed
public var rotationSpeed: float;

// this variable mainly affects the duration of deceleration
// lesser value will lenghten the duration of deceleration
public var lerpSpeed: float;

// these two variables determine the furthest tilt that the orbit will do
// they are relative to 0 value, so the more extreme these variables are, the higher (or lower) the tilt
public var minimumTilt:float = -30F;
public var maximumTilt:float = 30F;



function Start () {
	originalRotation = transform.localRotation;
	originalXRot = transform.localEulerAngles.x;
	originalYRot = transform.localEulerAngles.y;
}

function Update () {
	
	if(Input.GetMouseButton(0)) holdTimer++;
	
	// if the user hold for more than 3 frame, record the mouse y-axis
	if(Input.GetMouseButton(0) && holdTimer > 3) {
		// reverse the received y-axis to create inverse-axis movement (remove minus sign if you want normal-axis movement)
		yaxis = -Input.GetAxis("Mouse Y");
		vertSpeed = yaxis;
		xAxis = Input.GetAxis("Mouse X");
		horiSpeed = xAxis;
		
	} 
	// else the user is not holding the mouse click anymore, begin calculating the lerp speed 
	else {
		var ix = Time.deltaTime * lerpSpeed;
		vertSpeed = Mathf.Lerp(vertSpeed, 0, ix);
		horiSpeed = Mathf.Lerp(horiSpeed, 0, ix);
	}
	
	// if the user release the mouse/touch, reset the timer
	if(Input.GetMouseButtonUp(0)) {
		holdTimer = 0;
	}
	
	rotationY += vertSpeed * -rotationSpeed * 0.8;
	rotationY = ClampAngle (rotationY, -maximumTilt, -minimumTilt);
	var yQuaternion:Quaternion = Quaternion.AngleAxis (rotationY, Vector3.left);
	transform.localRotation = originalRotation * yQuaternion;
	
	transform.Rotate(0.0, horiSpeed * rotationSpeed,0.0,  Space.World);
}

// to clamp the angles so that the rotation will not behave erraticly
static function ClampAngle (angle : float, min : float, max : float) {
	if (angle < -360)
		angle += 360;
	if (angle > 360)
		angle -= 360;
	return Mathf.Clamp (angle, min, max);
}

function OnGUI() {

		// to change to the pan orbit scene
		if (GUI.Button(Rect(Screen.width - 100, 25, 75, 50), "Pan\nOrbit")){
			Application.LoadLevel (0);
		}
		
	}
