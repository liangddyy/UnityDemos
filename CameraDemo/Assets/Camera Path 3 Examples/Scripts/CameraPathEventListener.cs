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
/// This script is used in the Basic Example scene
/// It shows some basic event listening to the animator
/// Remember to clean up your event listening!!!
/// </summary>
public class CameraPathEventListener : MonoBehaviour
{
    [SerializeField]
    private CameraPathAnimator animator;

    void Awake()
    {
        
        if(animator == null)
        {
            Debug.LogError("No animator assgined the the event listener");
            Destroy(gameObject);
        }

        animator.AnimationStartedEvent += OnAnimationStarted;
        animator.AnimationPausedEvent += OnAnimationPaused;
        animator.AnimationStoppedEvent += OnAnimationStopped;
        animator.AnimationFinishedEvent += OnAnimationFinished;
        animator.AnimationLoopedEvent += OnAnimationLooped;
        animator.AnimationPingPongEvent += OnAnimationPingPonged;
        animator.AnimationCustomEvent += OnCustomEvent;

        animator.AnimationPointReachedEvent += OnPointReached;
        animator.AnimationPointReachedWithNumberEvent += OnPointReachedByNumber;
    }

    private void OnCustomEvent(string eventname)
    {
        Debug.Log("Custom Camera Path event: "+eventname);
    }

    private void OnAnimationStarted()
    {
        Debug.Log("The animation has begun");
    }

    private void OnAnimationPaused()
    {
        Debug.Log("The animation has been paused");
    }

    private void OnAnimationStopped()
    {
        Debug.Log("The animation has been stopped");
    }

    private void OnAnimationFinished()
    {
        Debug.Log("The animation has finished");
    }

    private void OnAnimationLooped()
    {
        Debug.Log("The animation has looped back to the start");
    }

    private void OnAnimationPingPonged()
    {
        Debug.Log("The animation has ping ponged into the other direction");
    }

    private void OnPointReached()
    {
        Debug.Log("A point was reached");
    }

    private void OnPointReachedByNumber(int pointNumber)
    {
        Debug.Log("The point " + pointNumber + " was reached");
    }
}