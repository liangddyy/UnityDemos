using UnityEngine;
using System.Collections;

/// <summary>
/// 类似与物体展示的旋转脚本
/// </summary>
public class CubeRotate : MonoBehaviour
{
    public GameObject obj;
    public float speed = 20.0f;

    private bool isMouseUp = false;
    private float xSpeed;
    private float startX;
    private float prevX;


    void Start()
    {
    }

    void OnEnable()
    {
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startX = Input.mousePosition.x;
            prevX = Input.mousePosition.x;
            isMouseUp = false;
        }
        if (Input.GetMouseButton(0))
        {
            xSpeed = Input.mousePosition.x - prevX;
            prevX = Input.mousePosition.x;

            obj.transform.Rotate(0, -xSpeed*Time.fixedDeltaTime*speed, 0);
        }

        if (Input.GetMouseButtonUp(0))
        {
            xSpeed = Input.mousePosition.x - prevX;
            isMouseUp = true;
        }

        if (isMouseUp)
        {
            obj.transform.Rotate(0, -xSpeed*Time.fixedDeltaTime*speed, 0);
            xSpeed *= 0.92f;
            if (Mathf.Abs(xSpeed) < 0.3f) xSpeed = 0;
        }
    }
}