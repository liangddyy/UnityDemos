using UnityEngine;
using System.Collections;

public class BasePlayer : MonoBehaviour
{

    public static float rotateSpeed = -200;
    public float minRadius;
    public float maxRadius;
    public float acceleration = 5f;
    int layerMask;
    void Start()
    {
        layerMask = LayerMask.GetMask("UI");
    }
    public void InitPlayer()
    {
        float[] f = GameManager.GetRadius();
        minRadius = f[0];
        maxRadius = f[1];

        GameManager.isReady();
    }

    RaycastHit2D hit;

    bool isWhosyourdaddy;//玩家被击中时将持续一定时间的无敌
    float tickTimer;

    int tickNum;//玩家被击中的次数
    float tickNumTimer;//每隔0.1秒清零计数；
    void Update()
    {
        if (GameManager.currentState != GameManager.GameState.Start)
        {
            return;
        }
        hit = Physics2D.Raycast(transform.position, transform.forward, 100, layerMask);

        if (isWhosyourdaddy)
        {
            tickTimer += Time.deltaTime;
            if (tickTimer >= 0.16f)
            {
                isWhosyourdaddy = false;
            }

        }
        else if (tickNum >= 1)
        {
            tickNumTimer += Time.deltaTime;
            if (tickNumTimer > 0.1f)
            {
                tickNumTimer = 0;
                tickNum = 0;
            }

        }



        if (hit.collider != null)
        {
            if (hit.transform.tag == "Bonus")
            {
                hit.transform.GetComponent<BoxCollider2D>().enabled = false;
                hit.transform.GetComponent<SquareScript>().PlayDiedAni();
                GameManager.PlayAudioOK();
            }
            else if (hit.transform.tag == "FireFly")
            {
                Destroy(hit.transform.gameObject);
                tickNum++;
                if (tickNum >= 3)
                {
                    isWhosyourdaddy = true;
                    tickTimer = 0;
                    tickNum = 0;
                }

                if (!isWhosyourdaddy)
                {
                    GameManager.PlayDashed();
                }
                GameManager.PlayAudioError();


            }
        }


    }

}
