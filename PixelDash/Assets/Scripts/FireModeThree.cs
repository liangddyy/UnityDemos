using UnityEngine;
using System.Collections;
using DG.Tweening;

public class FireModeThree : MonoBehaviour
{

    public float rotateSpeed = 150;
    public float moveSpeed = 300;
    RectTransform rect;
    float timer;
    public void InitFireFly(int code, float ti, float rotateAdd, int isFired)
    {
        rotateSpeed += rotateAdd;
        if (code == 1)
        {
            rotateSpeed = -rotateSpeed;
        }
        if (isFired == 1)
        {
            moveSpeed = -moveSpeed;
        }
        timer = ti;
        rect = GameManager.GetGameCnter();
        isRotate = true;
        transform.localPosition = new Vector3(30, 35, 0);
    }

    void Update()
    {
        if (GameManager.currentState == GameManager.GameState.Start && isRotate)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                FireRotate();
            }
        }

        if (GameManager.currentState != GameManager.GameState.Start)
        {
            this.gameObject.SetActive(false);
            Destroy(this.gameObject);
        }
    }

    bool isRotate;
    void FireRotate()
    {
        transform.RotateAround(rect.position, rect.forward, rotateSpeed * Time.deltaTime);
        currentRadius += moveSpeed * Time.deltaTime;
        Vector3 end = AddPlayerPosition(rect.position, transform.position, currentRadius);
        transform.position = end;

        if (transform.localPosition.y > 390)
        {
            isRotate = false;
            Destroy(this.gameObject);
        }
    }
    float currentRadius;

    Vector3 AddPlayerPosition(Vector3 centre, Vector3 player, float dis)
    {
        Vector3 normal = (player - centre).normalized;
        return normal * dis + centre;
    }
}
