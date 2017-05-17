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
/// This is the main script in the Third Person Camera exmaple
/// It creates a God of War style cam that follows the player while sticking to the path
/// </summary>
public class CPEPlayerPathFollow : MonoBehaviour
{
    public enum OrientationModes
    {
        none,
        lookAtTarget,
        lookAtPathDirection
    }

    [SerializeField]
    private OrientationModes _orientationMode = OrientationModes.lookAtTarget;

    [SerializeField]
    private Transform player;

    [SerializeField]
    private Transform cam;

    [SerializeField]
    private CameraPath path;

    private float lastPercent = 0;
    private bool ignoreNormalise = false;

    private int accuracy = 3;//the higher the more accurate by an order of magnitude but doesn't cost an order of magnitude! :o)

    /// <summary>
    /// This is used for crash bandicoot style games to make the camera lag behind the nearest point.
    /// </summary>
    [SerializeField]
    private float pathLag = 0.0f;

    //Set the initial position of the cam so we don't jump at the start of the demo
    void Start()
    {
        float nearestPercent = path.GetNearestPoint(player.position, ignoreNormalise, 5);
        lastPercent = nearestPercent;

        Vector3 nearestPoint = path.GetPathPosition(nearestPercent, ignoreNormalise);
        cam.position = nearestPoint;
        switch(_orientationMode)
        {
            case OrientationModes.none:
                //none
                break;

            case OrientationModes.lookAtTarget:
                cam.rotation = Quaternion.LookRotation(player.position - cam.position);
                break;

            case OrientationModes.lookAtPathDirection:
                cam.rotation = Quaternion.LookRotation(path.GetPathDirection(nearestPercent));
                break;
        }
    }

    //Update the cam animation 
    void LateUpdate()
    {
        float nearestPercent = path.GetNearestPoint(player.position, ignoreNormalise, accuracy);
        float theta = nearestPercent - lastPercent;
        if (theta > 0.5f)
            lastPercent += 1;
        else if (theta < -0.5f)
            lastPercent += -1;

        float usePercent = Mathf.Lerp(lastPercent, nearestPercent, 0.4f);
        lastPercent = usePercent;
        Vector3 nearestPoint = path.GetPathPosition(usePercent, ignoreNormalise);
        Vector3 backwards = -path.GetPathDirection(usePercent, !ignoreNormalise);

        cam.position = Vector3.Lerp(cam.position, nearestPoint + backwards * pathLag, 0.4f);

        switch (_orientationMode)
        {
            case OrientationModes.none:
                //none
                break;

            case OrientationModes.lookAtTarget:
                cam.rotation = Quaternion.LookRotation(player.position - cam.position);
                break;

            case OrientationModes.lookAtPathDirection:
                cam.rotation = Quaternion.LookRotation(path.GetPathDirection(usePercent));
                break;
        }
    }
}
