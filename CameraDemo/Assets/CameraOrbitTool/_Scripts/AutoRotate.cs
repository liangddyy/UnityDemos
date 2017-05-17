using UnityEngine;
using System.Collections;

public class AutoRotate : MonoBehaviour
{


    // variable that holds the auto-rotation speed
    // If the value is positive ( > 0 ), the rotation will be clockwise
    // if the value is negative ( < 0 ), the rotation will be counter-clockwise
    public float rotationSpeed = 3;

    void Start()
    {

    }

    void Update()
    {
        // rotate the object if the user is not clicking or touching the screen
        if (!Input.GetMouseButton(0)) transform.Rotate(0.0f, rotationSpeed, 0.0f, Space.World);
    }
}