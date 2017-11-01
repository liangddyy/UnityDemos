using UnityEngine;
using System.Collections;

public class FullPlayer : MonoBehaviour
{

    RectTransform target;

    float minRadius;
    float maxRadius;
    float currentRadius;
    float acceleration;

    public void InitPlayerController(RectTransform rec)
    {
        minRadius = GetComponent<BasePlayer>().minRadius;
        maxRadius = GetComponent<BasePlayer>().maxRadius;
        if (Application.platform == RuntimePlatform.Android)
        {
            acceleration = 2.8f * GetComponent<BasePlayer>().acceleration;
        }
        else
        {
            acceleration = GetComponent<BasePlayer>().acceleration;
        }
        target = rec;
        GameManager.isReady();
    }
    void Update()
    {
        if (GameManager.currentState != GameManager.GameState.Start)
        {
            return;
        }
        if (GameManager.isPressedRight)
        {
            currentRadius += acceleration;
            currentRadius = Mathf.Clamp(currentRadius, minRadius, maxRadius);
            Vector3 end = AddPlayerPosition(target.position, transform.position, currentRadius);
            transform.position = end;
        }
        else
        {
            currentRadius -= acceleration;
            currentRadius = Mathf.Clamp(currentRadius, minRadius, maxRadius);
            Vector3 end = AddPlayerPosition(target.position, transform.position, currentRadius);
            transform.position = end;

        }
        if (GameManager.isPressedLeft)
        {
            BasePlayer.rotateSpeed = -Mathf.Abs(BasePlayer.rotateSpeed);
        }
        else
        {
            BasePlayer.rotateSpeed = Mathf.Abs(BasePlayer.rotateSpeed);
        }
       
    }
    void LateUpdate()
    {
        if (GameManager.currentState == GameManager.GameState.Start)
        {
            RotatePlayer();
        }
    }

    void RotatePlayer()
    {
        transform.RotateAround(target.position, target.forward, BasePlayer.rotateSpeed * Time.deltaTime);
    }

    Vector3 AddPlayerPosition(Vector3 centre, Vector3 player, float dis)
    {
        Vector3 normal = (player - centre).normalized;
        return normal * dis + centre;
    }
}
