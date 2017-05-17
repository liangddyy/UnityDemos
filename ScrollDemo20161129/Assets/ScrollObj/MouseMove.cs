using UnityEngine;
using System.Collections;

public class MouseMove : MonoBehaviour {

    //鼠标经过时改变物体颜色  
    private Color mouseOverColor = Color.blue;//声明变量为蓝色  
    private Color originalColor;//声明变量来存储本来颜色  

    void Start()
    {
        
        originalColor = GetComponent<Renderer>().sharedMaterial.color;//开始时得到物体着色  
    }

    void OnMouseEnter()
    {
        GetComponent<Renderer>().material.color = mouseOverColor;//当鼠标滑过时改变物体颜色为蓝色  
    }

    void OnMouseExit()
    {
        GetComponent<Renderer>().material.color = originalColor;//当鼠标滑出时恢复物体本来颜色  
    }

    IEnumerator OnMouseDown()
    {
        
        Vector3 screenSpace = Camera.main.WorldToScreenPoint(transform.position);//三维物体坐标转屏幕坐标  
        //将鼠标屏幕坐标转为三维坐标，再计算物体位置与鼠标之间的距离  
        var offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z));
        print("down");
        while (Input.GetMouseButton(0))
        {
            Vector3 curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);
            var curPosition = Camera.main.ScreenToWorldPoint(curScreenSpace) + offset;
            transform.position = curPosition;
            yield return new WaitForFixedUpdate();
        }
    }
}
