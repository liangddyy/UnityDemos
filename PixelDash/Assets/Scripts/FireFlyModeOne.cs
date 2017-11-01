using UnityEngine;
using System.Collections;
using DG.Tweening;

public class FireFlyModeOne : MonoBehaviour
{

    Vector3 pos;
    float speed;

    public void InitFireFly(float spe, Vector3 po, float wait)
    {
        pos = po;
        speed = spe;
        cd = wait;

        if (wait <= 0)
        {
            Tweener twr = transform.DOLocalMove(pos, speed);
            twr.SetEase(Ease.Linear);
            twr.OnComplete(FinishedFireFly);
        }
        else
        {
            isLater = true;
        }
    }

    void FinishedFireFly()
    {
        Destroy(this.gameObject);
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
                Tweener twr = transform.DOLocalMove(pos, speed);
                twr.SetEase(Ease.Linear);
                twr.OnComplete(FinishedFireFly);
            }
        }
        if (GameManager.currentState != GameManager.GameState.Start)
        {
            this.gameObject.SetActive(false);
            Destroy(this.gameObject);
        }
    }
}
