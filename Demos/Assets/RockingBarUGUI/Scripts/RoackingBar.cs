using UnityEngine;
using System.Collections;

public class RoackingBar : MonoBehaviour
{
    public RectTransform rectViewport;  // 用于计算radius
    public RectTransform rectCenter;    // 获取摇杆坐标
    private int radius;

    void Start()
    {
        radius = (int) rectViewport.sizeDelta.x/2;
    }

    public void On_Move(RectTransform rect)
    {
        // magnitude 向量长度
        if (rect.anchoredPosition.magnitude > radius)
        {
            // 将摇杆 限制在圆形面板内
            rect.anchoredPosition = rect.anchoredPosition.normalized*radius;
        }
    }
}