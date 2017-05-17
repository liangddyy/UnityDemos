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

/// <summary>
/// This script is used in the On Rails Shooter example.
/// It is the main script for the targets you must shoot.
/// </summary>
public class CameraPathOnRailsTarget : MonoBehaviour
{

    private enum States
    {
        Off,
        Show,
        Hide
    }

    [SerializeField]
    private GameObject paper;

    private States state = States.Off;
    private Vector3 defaultPosition;
    private Quaternion flat;
    private Vector3 flatPosition;
    private float lerpSpeed = 0.3f;
    private BoxCollider boxCollider;

    public void Show()
    {
        boxCollider.enabled = true;
        state = States.Show;
    }

    public void Hide()
    {
        if(state != States.Hide)
        {
            boxCollider.enabled = false;
            state = States.Hide;
        }
    }

    private void OnMouseDown()
    {
        boxCollider.enabled = false;
        state = States.Hide;
    }

    private void Awake()
    {
        defaultPosition = paper.transform.localPosition;
        Vector3 reposition = paper.transform.localPosition;
        reposition.y = -reposition.y;
        paper.transform.localPosition = reposition;
        flat = Quaternion.Euler(90, 0, 0);

        flatPosition = new Vector3(0, 0, defaultPosition.y);
        boxCollider = gameObject.GetComponent<BoxCollider>();
        boxCollider.enabled = false;
    }

    private void Update()
    {
        switch(state)
        {
                case States.Off:
                //do nothing
                break;

                case States.Show:
                paper.transform.localPosition = Vector3.Lerp(paper.transform.localPosition, defaultPosition, lerpSpeed);
                break;

                case States.Hide:
                paper.transform.localRotation = Quaternion.Lerp(paper.transform.localRotation, flat, lerpSpeed);
                paper.transform.localPosition = Vector3.Lerp(paper.transform.localPosition, flatPosition, lerpSpeed);
                break;
        }
    }
}
