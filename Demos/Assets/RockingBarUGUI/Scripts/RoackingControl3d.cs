using UnityEngine;
using System.Collections;

public class RoackingControl3d : MonoBehaviour
{
    [SerializeField] private RoackingBar roackingBar;
    // Use this for initialization
    void Start()
    {
        roackingBar = GameObject.Find("RockingBar").GetComponent<RoackingBar>(); //给摇杆UI赋值
    }

    // Update is called once per frame
    void Update()
    {
        // 移动物体
        transform.Translate((roackingBar.rectCenter.anchoredPosition.x/10)*Time.deltaTime, 0,
            (roackingBar.rectCenter.anchoredPosition.y/10)*Time.deltaTime); 
    }
}