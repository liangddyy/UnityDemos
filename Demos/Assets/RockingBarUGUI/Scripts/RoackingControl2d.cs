using UnityEngine;
using System.Collections;

/// <summary>这个脚本写的是如何用 摇杆来控制2D
/// </summary>
public class RoackingControl2d : MonoBehaviour
{
    [SerializeField] private RectTransform rectPlayer; //拖拽赋值
    [SerializeField] private RoackingBar roackingBar; //拖拽赋值

    // Update is called once per frame
    void Update()
    {
        rectPlayer.anchoredPosition += (roackingBar.rectCenter.anchoredPosition/10); //2D坐标 += 摇杆的坐标变化值/10
    }
}