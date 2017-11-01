using UnityEngine;
using System.Collections;


public class AutoPlayer : MonoBehaviour
{
    // Use this for initialization
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

    // Update is called once per frame
    void Update()
    {

        if (GameManager.currentState != GameManager.GameState.Start)
        {
            return;
        }
        if (GameManager.isPressedLeft || GameManager.isPressedRight)
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
