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
/// This class is used in the On Rails Shooter exmaple.
/// It listens for an event "Collapse" on the path and triggers debris to drop and block the player
/// </summary>
public class CameraPathOnRailsTriggerCollapse : MonoBehaviour 
{

    [SerializeField]
    private CameraPathAnimator animator;

    void Awake()
    {

        if (animator == null)
        {
            Debug.LogError("No animator assgined the the event listener");
            Destroy(gameObject);
            return;
        }

        animator.AnimationCustomEvent += OnCustomEvent;

        Rigidbody[] rigidbodies = transform.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = true;
        }
    }

    private void OnCustomEvent(string eventname)
    {
        switch (eventname)
        {
            case "Collapse":
                Rigidbody[] rigidbodies = transform.GetComponentsInChildren<Rigidbody>();
                foreach (Rigidbody rb in rigidbodies)
                {
                    rb.isKinematic = false;
                    rb.useGravity = true;
                    rb.AddForce(Vector3.down);
                }
                break;
        }
    }
}
