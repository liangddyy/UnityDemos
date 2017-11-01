using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class SquareScript : MonoBehaviour
{



    public void PlayDiedAni()
    {
        GameManager.DashSquare();
        Tweener tweener = transform.DOScale(1.5f, 0.6f);
        transform.GetComponent<Image>().DOColor(Color.grey, 0.5f);
        tweener.OnComplete(Dead);
        
    }
    void Dead()
    {
        GameManager.CreatSquare();
        Destroy(this.gameObject);
    }



}
