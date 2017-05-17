// variable that determine the speed of rotation
var rotationSpeed:float;

// variable that determine the speed of lerping (deceleration)
var lerpSpeed:float;

// variable that holds the current rotation speed
private var speed:float;

// timer to check whether the touch is a valid rotation, to prevent camera jerking on mobile device
private var holdTimer:float = 0.0;

// variable to hold the x-axis from mouse
private var xAxis:float = 0.0;
private var lastTouch:int = 0;

function Update () {
	if(Input.touchCount < 2 && lastTouch < 2) {
		if(Input.touchCount == 0) lastTouch = 0;
		// adds up the timer everytime the user holds the left mouse button/one finger touch in mobile device
		if(Input.GetMouseButton(0)) holdTimer++;
		
		// if the user hold for more than 3 frame, record the mouse x-axis
		if (Input.GetMouseButton(0) && holdTimer > 3)
		{
			holdTimer ++;
			xAxis = Input.GetAxis("Mouse X");
			speed = xAxis;
		} 
		// else the user is not holding the mouse click anymore, begin calculating the lerp speed 
		else {
			var i = Time.deltaTime * lerpSpeed;
			speed = Mathf.Lerp(speed, 0, i);
		}
		
		// if the user release the mouse/touch, reset the timer
		if (Input.GetMouseButtonUp(0))
		{
			holdTimer = 0;
		}
		// rotate the object
		transform.Rotate(0.0, speed * rotationSpeed,0.0,  Space.World);
	} else {
		lastTouch = Input.touchCount;
		speed = 0;
	}
}