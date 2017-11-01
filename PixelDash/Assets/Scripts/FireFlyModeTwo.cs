using UnityEngine;
using System.Collections;
using DG.Tweening;

public class FireFlyModeTwo : MonoBehaviour
{
    int index = 0;
    // Use this for initialization

    Vector3 endPos;
    Tweener twr;
    float speed;
    public void InitFireFly(float spe, Vector3 pos)
    {
        endPos = pos;
        speed = spe;
        twr = transform.DOLocalMove(endPos.normalized * 130, 0.85f);
        //Tweener twr2 = transform.DOLocalMove(pos, speed);
        twr.SetEase(Ease.OutBack);
        twr.OnComplete(FinishedFireFly);
    }
    public void InitFireFly(float spe, Vector3 pos, float wait)
    {
        endPos = pos;
        speed = spe;
        if (wait <= 0)
        {
            InitFireFly(spe, pos);
        }
        else
        {
            cd = wait;
            isLater = true;
        }


    }

    void FinishedFireFly()
    {
        index++;
        if (index < 6)
        {
            twr = transform.DOLocalMove(endPos.normalized * 120 * (index + 1), 0.9f);
            twr.SetEase(Ease.OutBack);
            twr.OnComplete(FinishedFireFly);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    float tickTimer;
    bool isLater;
    float cd;

    //Update is called once per frame
    void Update()
    {
        if (isLater)
        {
            tickTimer += Time.deltaTime;
            if (tickTimer >= cd)
            {
                isLater = false;
                InitFireFly(speed, endPos);
            }
        }

        if (GameManager.currentState != GameManager.GameState.Start)
        {
            this.gameObject.SetActive(false);
            Destroy(this.gameObject);
        }
    }

}
