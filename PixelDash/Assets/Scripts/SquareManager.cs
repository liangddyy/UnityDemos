using UnityEngine;
using System.Collections;

public class SquareManager : MonoBehaviour
{

    public GameObject squareProfab;
    public static GameObject currentSquare;
    static GameObject sSquareProfab;

    //static float minRadius;
    //static float maxRadius;

    static int[] perD;
    public void InitSquareManager()
    {
        sSquareProfab = squareProfab;
        //float[] f = GameManager.GetRadius();
        //minRadius = f[0];
        //maxRadius = f[1];
        perD = new int[] { 123, 180, 241, 300 };
    }

    public void CreatSquare()
    {
        if (GameManager.currentState == GameManager.GameState.Start && sSquareProfab)
        {
            if (currentSquare != null)
            {
                Destroy(currentSquare.gameObject);
            }
            currentSquare = GameObject.Instantiate(sSquareProfab);
            currentSquare.transform.SetParent(GameManager.GetParent());
            currentSquare.transform.localScale = Vector3.one;
            currentSquare.transform.localPosition = GetPos();
        }
    }

    static Vector3 GetPos()
    {
        int i = Random.Range(0, 4);
        Vector3 v1 = new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), 0);
        Vector3 normal = (v1 - Vector3.zero).normalized;
        Vector3 pos = normal * perD[i] + new Vector3(0, 10, 0);

        return pos;
    }

    public  void DestroySquare()
    {
        if (currentSquare)
        {
            Destroy(currentSquare.gameObject);
        }

    }
    //float tickTimer;
    //void Update()
    //{
    //    tickTimer += Time.deltaTime;
    //    if (tickTimer >= 2)
    //    {
    //        tickTimer = 0;
    //        if (currentSquare != null)
    //        {
    //            Destroy(currentSquare.gameObject);
    //        }
    //        currentSquare = GameObject.Instantiate(sSquareProfab);
    //        currentSquare.transform.SetParent(GameManager.GetParent());
    //        currentSquare.transform.localScale = Vector3.one;
    //        currentSquare.transform.localPosition = GetPos();
    //    }

    //}

}
