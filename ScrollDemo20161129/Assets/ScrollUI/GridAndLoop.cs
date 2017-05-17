using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class GridAndLoop : MonoBehaviour
{

    public enum ArrangeType
    {
        Vertical = 0, //垂直排列
        Horizontal = 1 //水平排列
        
    }

    public ArrangeType arrangeType = ArrangeType.Horizontal;

    public int cell_x = 120, cell_y = 100;

    private readonly Vector3[] conners = new Vector3[4]; //ScrollRect四角的世界坐标 

    /// <summary>
    ///     行列个数 0表示1列
    /// </summary>
    public int ConstraintCount;

    /// <summary>
    ///     是否隐藏裁剪部分
    /// </summary>
    public bool cullContent = true;

    /// <summary>
    ///     显示区域长度或高度的一半
    /// </summary>
    private float extents;
    public int maxIndex = 0;

    private List<Transform> mChild;

    private bool mHorizontal;//滚动方向

    public int minIndex;

    /// <summary>
    ///     当前RectTransform对象
    /// </summary>
    private RectTransform rectTransform;

    private ScrollRect scrollRect;

    /// <summary>
    ///     当前对象
    /// </summary>
    private Transform mTrans;


    private Vector2 SR_size = Vector2.zero; //SrollRect的尺寸
    //private Vector2 startPos; //ScrollRect的初始位置


    void Start()
    {
        mChild = new List<Transform>();

        InitList();
    }

    private void InitList()
    {
        int i, ChildCount;
        InitValue();
        mChild.Clear();

        for (i = 0, ChildCount = mTrans.childCount; i < ChildCount; i++)
            mChild.Add(mTrans.GetChild(i));

        ResetChildPosition();
        //     mChild.Sort(sortByName);//按照Item名字排序 
    }

    private void InitValue()
    {
        if (ConstraintCount <= 0)
            ConstraintCount = 1;
        if (minIndex > maxIndex) minIndex = maxIndex;
        mTrans = transform;
        rectTransform = transform.GetComponent<RectTransform>();
        scrollRect = transform.parent.GetComponent<ScrollRect>();
        mHorizontal = scrollRect.horizontal;

        SR_size = transform.parent.GetComponent<RectTransform>().rect.size;

        //四角坐标  横着数
        conners[0] = new Vector3(-SR_size.x/2f, SR_size.y/2f, 0);
        conners[1] = new Vector3(SR_size.x/2f, SR_size.y/2f, 0);
        conners[2] = new Vector3(-SR_size.x/2f, -SR_size.y/2f, 0);
        conners[3] = new Vector3(SR_size.x/2f, -SR_size.y/2f, 0);
        for (var i = 0; i < 4; i++)
        {
            var temp = transform.parent.TransformPoint(conners[i]);
            conners[i].x = temp.x;
            conners[i].y = temp.y;
        }

        rectTransform.pivot = new Vector2(0, 1); //设置panel的中心在左上角

        
        scrollRect.onValueChanged.AddListener(delegate { WrapContent(); }); //添加滚动事件回调
        //startPos = mTrans.localPosition;
    }

    void Update()
    {
        if (Application.isPlaying)
        {
            enabled = false;
        }
        //RePosition();
        //InitList();
    }

    private void ResetChildPosition()
    {
        int rows = 1, cols = 1;

        var startAxis = new Vector2(cell_x/2f, -cell_y/2f); //起始位置
        int i;

        var imax = mChild.Count; //Item元素数量

        //初始化行列数
        if (arrangeType == ArrangeType.Horizontal) 
        {
            rows = ConstraintCount;
            cols = (int) Mathf.Ceil(imax/(float) rows);

            extents = cols*cell_x*0.5f;
        }
        else if (arrangeType == ArrangeType.Vertical)
        {
            cols = ConstraintCount;
            rows = (int) Mathf.Ceil(imax/(float) cols);
            extents = rows*cell_y*0.5f;
        }

        for (i = 0; i < imax; i++)
        {
            var temp = mChild[i];

            int x = 0, y = 0; //行列号
            if (arrangeType == ArrangeType.Vertical)
            {
                x = i/cols;
                y = i%cols;
            }
            else if (arrangeType == ArrangeType.Horizontal)
            {
                x = i%rows;
                y = i/rows;
            }


            temp.localPosition = new Vector2(startAxis.x + y*cell_x, startAxis.y - x*cell_y);

            if ((minIndex == maxIndex) || ((i >= minIndex) && (i <= maxIndex)))
            {
                cullContent = true;
                temp.gameObject.SetActive(true);
                //UpdateRectsize(temp.localPosition); //更新panel的尺寸

                // UpdateItem(temp, i, i);
            }
            else
            {
                cullContent = false;
                temp.gameObject.SetActive(false); //如果预制Item数超过maxIndex则将超过部分隐藏 并 设置cullCintent为ufalse 并且不再更新 panel尺寸
            }
        }
    }

    private void UpdateRectsize(Vector2 pos)
    {
        if (arrangeType == ArrangeType.Horizontal)
        {
            //rectTransform.offsetMax
            rectTransform.sizeDelta = new Vector2(pos.x + cell_x, ConstraintCount*cell_y);
            
        }
        else
            rectTransform.sizeDelta = new Vector2(ConstraintCount*cell_x, -pos.y + cell_y);
    }

    private int getRealIndex(Vector2 pos) //计算realindex
    {
        var x = (int) Mathf.Ceil(-pos.y/cell_y) - 1; //行号
        var y = (int) Mathf.Ceil(pos.x/cell_x) - 1; //列号

        int realIndex;
        if (arrangeType == ArrangeType.Horizontal) realIndex = x*ConstraintCount + y;
        else realIndex = x + ConstraintCount*y;

        return realIndex;
    }

    //回调
    private void WrapContent()
    {
        var conner_local = new Vector3[4];
        for (var i = 0; i < 4; i++)
            conner_local[i] = mTrans.InverseTransformPoint(conners[i]);
        //计算ScrollRect的中心坐标 相对于this的坐标
        Vector2 center = (conner_local[3] + conner_local[0])/2f;


        if (mHorizontal)
        {
            Debug.Log("横向");
            var min = conner_local[0].x - cell_x; //显示区域
            var max = conner_local[3].x + cell_x;
            for (int i = 0, imax = mChild.Count; i < imax; i++)
            {
                var temp = mChild[i];
                var distance = temp.localPosition.x - center.x;

                if (distance < -extents)
                {
                    Vector2 pos = temp.localPosition;
                    pos.x += extents*2f;

                    var realIndex = getRealIndex(pos);

                    if ((minIndex == maxIndex) || ((realIndex >= minIndex) && (realIndex < maxIndex)))
                    {
                        UpdateRectsize(pos);
                        temp.localPosition = pos;
                        //设置Item内容
                        //UpdateItem(temp, i, realIndex);       
                    }
                }

                if (distance > extents)
                {
                    Vector2 pos = temp.localPosition;
                    pos.x -= extents*2f;

                    var realIndex = getRealIndex(pos);

                    if ((minIndex == maxIndex) || ((realIndex >= minIndex) && (realIndex < maxIndex)))
                    {
                        temp.localPosition = pos;
                    }
                }

                if (cullContent) //设置裁剪部分是否隐藏
                {
                    Vector2 pos = temp.localPosition;
                    temp.gameObject.SetActive((pos.x > min) && (pos.x < max) ? true : false);
                }
            }
        }
//        else
//        {
//            var min = conner_local[3].y - cell_y; //显示区域
//            var max = conner_local[0].y + cell_y;
//            for (int i = 0, imax = mChild.Count; i < imax; i++)
//            {
//                var temp = mChild[i];
//                var distance = temp.localPosition.y - center.y;
//
//                if (distance < -extents)
//                {
//                    Vector2 pos = temp.localPosition;
//                    pos.y += extents*2f;
//
//                    var realIndex = getRealIndex(pos);
//
//                    if ((minIndex == maxIndex) || ((realIndex >= minIndex) && (realIndex < maxIndex)))
//                        temp.localPosition = pos;
//                }
//
//                if (distance > extents)
//                {
//                    Vector2 pos = temp.localPosition;
//                    pos.y -= extents*2f;
//
//                    var x = (int) Mathf.Ceil(-pos.y/cell_y) - 1; //行号
//                    var y = (int) Mathf.Ceil(pos.x/cell_x) - 1; //列号
//
//                    int realIndex;
//                    if (arrangeType == ArrangeType.Horizontal) realIndex = x*ConstraintCount + y;
//                    else realIndex = x + ConstraintCount*y;
//
//                    if ((minIndex == maxIndex) || ((realIndex >= minIndex) && (realIndex < maxIndex)))
//                    {
//                        //UpdateRectsize(pos);
//
//                        temp.localPosition = pos;
//
//                        //设置Item内容
//                        //UpdateItem(temp, i, realIndex);
//                    }
//                }
//                if (cullContent) //设置裁剪部分是否隐藏
//                {
//                    Vector2 pos = temp.localPosition;
//                    temp.gameObject.SetActive((pos.y > min) && (pos.y < max) ? true : false);
//                }
//            }
//        }
    }
}