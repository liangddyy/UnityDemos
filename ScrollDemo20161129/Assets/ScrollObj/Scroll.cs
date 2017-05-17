using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Scroll : MonoBehaviour
{
    //private int childNum;
    private List<Transform> childTransforms;
    private Vector3 fistVector3;
    private Vector3 lastVector3;
    private bool isMouseDown;
    private Vector3 lastMousePosition;

    private float leftRightDistance;
    private float minDistance = 1000f;
    private int minDistanceIndex = -1;
    //private Vector3 oldPosition;

    public float step = 2f; //间距

    private Transform transform;

    public float width = 4f;

    // Use this for initialization
    private void Start()
    {
        transform = GetComponent<Transform>();
        childTransforms = new List<Transform>();

        for (var i = 0; i < transform.childCount; i++)
            childTransforms.Add(transform.GetChild(i));
        initChildsPos();
    }

    private void initChildsPos()
    {
        for (var i = 0; i < childTransforms.Count; i++)
            childTransforms[i].position = new Vector3(transform.position.x - width + step * i, 0, 0);
        leftRightDistance = step * childTransforms.Count - leftRightDistance;
        fistVector3 = childTransforms[0].position;
        lastVector3 = childTransforms[childTransforms.Count - 1].position;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            isMouseDown = true;
        if (Input.GetMouseButtonUp(0))
        {
            if (isMouseDown)
            {
                isMouseDown = false;
                for (var i = 0; i < childTransforms.Count; i++)
                {
                    var dis = childTransforms[i].position.x - transform.position.x;
                    if (Mathf.Abs(dis) < Mathf.Abs(minDistance))
                    {
                        minDistanceIndex = i;
                        minDistance = dis;
                    }
                }
                if (minDistanceIndex != -1)
                {
                    var vector3 = transform.position - childTransforms[minDistanceIndex].position;

                    print("偏移量" + minDistance);
                    foreach (var trans in childTransforms)
                        trans.DOMoveX(trans.position.x - minDistance, 0.5f);
                    minDistanceIndex = -1;
                    minDistance = 1000;
                }
            }
            lastMousePosition = Vector3.zero;
        }


        if (isMouseDown)
        {
            if (lastMousePosition != Vector3.zero)
            {
                var offset = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, 0, 0)) -
                             lastMousePosition;

                for (var i = 0; i < childTransforms.Count; i++)
                    childTransforms[i].position = checkPosition(childTransforms[i].position + offset);
            }
            lastMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, 0, 0));
        }
    }

    private Vector3 checkPosition(Vector3 vector3)
    {
        if (vector3.x < fistVector3.x)
            vector3.x += leftRightDistance;
        if (vector3.x > lastVector3.x)
            vector3.x -= leftRightDistance;
        return vector3;
    }

    private struct MyStruct
    {
        public float width;
        public float height;
    }

    //                childTransforms[i].position = curPosition;

    //            offset[i] = childTransforms[i].position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, 0, screenSpace[i].z));
    //            screenSpace[i] = Camera.main.WorldToScreenPoint(childTransforms[i].position);//三维物体坐标转屏幕坐标
    //        {
    //        for (int i = 0; i < childNum; i++)
    //
    //        Vector3[] offset = new Vector3[childNum];
    //        //将鼠标屏幕坐标转为三维坐标，再计算物体位置与鼠标之间的距离  
    //        Vector3 []screenSpace = new Vector3[childNum];  
    //    {
    //    IEnumerator OnMouseDown()
    //

    //        }
    //        
    //        while (Input.GetMouseButton(0))
    //        {
    //            for (int i = 0; i < childNum; i++)
    //            {
    //                Vector3 curScreenSpace = new Vector3(Input.mousePosition.x, 0, screenSpace[i].z);

    //                var curPosition = Camera.main.ScreenToWorldPoint(curScreenSpace) + offset[i];
    //            }
    //            
    //            yield return new WaitForFixedUpdate();
    //        }
    //    }
}