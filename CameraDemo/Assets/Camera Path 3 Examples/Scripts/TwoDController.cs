using UnityEngine;
using System.Collections;

public class TwoDController : MonoBehaviour
{
    private bool followPlayer = false;

    [SerializeField]
    private PlayerControl playerControl;

    [SerializeField]
    private Transform targetCamera;

    [SerializeField]
    private CameraPath gameplayPath;

    [SerializeField]
    private Transform player;
    private float lastPercent = 0;

    public void Start()
    {
        playerControl.enabled = false;
    }

    public void StartGame()
    {
        playerControl.enabled = true;
        followPlayer = true;
    }

    private void LateUpdate()
    {
        if(!followPlayer)
            return;
        float nearestPercent = gameplayPath.GetNearestPoint(player.position, false);
        float theta = nearestPercent - lastPercent;
        if(theta > 0.5f)
            lastPercent += 1;
        else if(theta < -0.5f)
            lastPercent += -1;

        float usePercent = Mathf.Lerp(lastPercent, nearestPercent, 0.4f);
        lastPercent = usePercent;
        Vector3 nearestPoint = gameplayPath.GetPathPosition(usePercent, false);

        targetCamera.position = Vector3.Lerp(targetCamera.position, nearestPoint, 0.4f);
    }
}
