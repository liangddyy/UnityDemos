#pragma strict

// variable that holds the auto-rotation speed
// If the value is positive ( > 0 ), the rotation will be clockwise
// if the value is negative ( < 0 ), the rotation will be counter-clockwise
var rotationSpeed:float = 3;

function Start () {

}

function Update () {
    // rotate the object if the user is not clicking or touching the screen
    if(!Input.GetMouseButton(0)) transform.Rotate(0.0, rotationSpeed, 0.0,  Space.World);
}