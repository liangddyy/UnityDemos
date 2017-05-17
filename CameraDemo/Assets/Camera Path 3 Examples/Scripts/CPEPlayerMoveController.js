#pragma strict

// Objects to drag in
public var motor : CPEMovementMotor;
public var character : Transform;
public var joystickPrefab : GameObject;

// Settings
public var cameraSmoothing : float = 0.01;
public var cameraPreview : float = 2.0f;

// Cursor settings
public var cursorPlaneHeight : float = 0;
public var cursorFacingCamera : float = 0;
public var cursorSmallerWithDistance : float = 0;
public var cursorSmallerWhenClose : float = 1;

private var cursorObject : Transform;

private var mainCameraTransform : Transform;
private var cameraVelocity : Vector3 = Vector3.zero;
private var cameraOffset : Vector3 = Vector3.zero;
private var initOffsetToPlayer : Vector3;

// Prepare a cursor point varibale. This is the mouse position on PC and controlled by the thumbstick on mobiles.
private var cursorScreenPosition : Vector3;

private var playerMovementPlane : Plane;

private var joystickRightGO : GameObject;

private var screenMovementSpace : Quaternion;
private var screenMovementForward : Vector3;
private var screenMovementRight : Vector3;

function Awake () {		
	motor.movementDirection = Vector2.zero;
	motor.facingDirection = Vector2.zero;
	
	// Ensure we have character set
	// Default to using the transform this component is on
	if (!character)
	    character = transform;
	
	// Save camera offset so we can use it in the first frame
	
	// Set the initial cursor position to the center of the screen
	cursorScreenPosition = Vector3 (0.5 * Screen.width, 0.5 * Screen.height, 0);
	
	// caching movement plane
	playerMovementPlane = new Plane (character.up, character.position + character.up * cursorPlaneHeight);
}

function Update () 
{
    screenMovementSpace = Quaternion.Euler (0, Camera.main.transform.rotation.eulerAngles.y, 0);
    screenMovementForward = screenMovementSpace * Vector3.forward;
    screenMovementRight = screenMovementSpace * Vector3.right;	

	motor.movementDirection = Input.GetAxis ("Horizontal") * screenMovementRight + Input.GetAxis ("Vertical") * screenMovementForward;
	
	// Make sure the direction vector doesn't exceed a length of 1
	// so the character can't move faster diagonally than horizontally or vertically
	if (motor.movementDirection.sqrMagnitude > 1)
		motor.movementDirection.Normalize();
	
	
	// HANDLE CHARACTER FACING DIRECTION AND SCREEN FOCUS POINT
	
	// First update the camera position to take into account how much the character moved since last frame
	//mainCameraTransform.position = Vector3.Lerp (mainCameraTransform.position, character.position + cameraOffset, Time.deltaTime * 45.0f * deathSmoothoutMultiplier);
	
	// Set up the movement plane of the character, so screenpositions
	// can be converted into world positions on this plane
	//playerMovementPlane = new Plane (Vector3.up, character.position + character.up * cursorPlaneHeight);
	
	// optimization (instead of newing Plane):
	
	playerMovementPlane.normal = character.up;
	playerMovementPlane.distance = -character.position.y + cursorPlaneHeight;
	
	// used to adjust the camera based on cursor or joystick position
	
	var cameraAdjustmentVector : Vector3 = Vector3.zero;
	
	#if !UNITY_EDITOR && (UNITY_XBOX360 || UNITY_PS3)

		// On consoles use the analog sticks
		var axisX : float = Input.GetAxis("LookHorizontal");
		var axisY : float = Input.GetAxis("LookVertical");
		motor.facingDirection = axisX * screenMovementRight + axisY * screenMovementForward;
	
		cameraAdjustmentVector = motor.facingDirection;		
		
	#else
	
		// On PC, the cursor point is the mouse position
		var cursorScreenPosition : Vector3 = Input.mousePosition;
						
		// Find out where the mouse ray intersects with the movement plane of the player

			
		var halfWidth : float = Screen.width / 2.0f;
		var halfHeight : float = Screen.height / 2.0f;
		var maxHalf : float = Mathf.Max (halfWidth, halfHeight);
			
		// Acquire the relative screen position			
		var posRel : Vector3 = cursorScreenPosition - Vector3 (halfWidth, halfHeight, cursorScreenPosition.z);		
		posRel.x /= maxHalf; 
		posRel.y /= maxHalf;
						
		cameraAdjustmentVector = posRel.x * screenMovementRight + posRel.y * screenMovementForward;
		cameraAdjustmentVector.y = 0.0;	
			
	#endif
		
	// HANDLE CAMERA POSITION
		
	// Set the target position of the camera to point at the focus point
	var cameraTargetPosition : Vector3 = character.position + initOffsetToPlayer + cameraAdjustmentVector * cameraPreview;
	
	// Apply some smoothing to the camera movement
	
	// Save camera offset so we can use it in the next frame
}

public static function PlaneRayIntersection (plane : Plane, ray : Ray) : Vector3 {
	var dist : float;
	plane.Raycast (ray, dist);
	return ray.GetPoint (dist);
}

public static function ScreenPointToWorldPointOnPlane (screenPoint : Vector3, plane : Plane, camera : Camera) : Vector3 {
	// Set up a ray corresponding to the screen position
	var ray : Ray = camera.ScreenPointToRay (screenPoint);
	
	// Find out where the ray intersects with the plane
	return PlaneRayIntersection (plane, ray);
}
