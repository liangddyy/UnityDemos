using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

/*该代码不懂可以问我+QQ2565190823，大家一起学习。*/
public class Ui_S : MonoBehaviour {
    public string sprit_Path = "Sprites/XiaoGuo";//Resources文件夹里的资源  加载方法加载Sprit图片集和的路径,可以在Inspector面板赋值
    public Sprite[] sp_S;//所有的UI图片
    public Button[] all_Btn;//用来获取所有的图块
    public Transform bg_T;//携带网格自动布局组建的Image背景。可以拖拽赋值
    private GridLayoutGroup bg_Layout;
    public GameObject btn_Gm;//用来实例化小图块的预制件，可以拖拽赋值
    public RectTransform null_Img;//空图片的位置
    public string all_SpritName;//正确的图片名字相加。
    public string all_BtnSpriteName;//用来累加当前所有图片的序列。  对比正确序列是否达到了
	// Use this for initialization
	void Start () {
        sp_S = Resources.LoadAll<Sprite>(sprit_Path);
        bg_Layout = bg_T.GetComponent<GridLayoutGroup>();

        List<Sprite> test_L = new List<Sprite>();
        for (int i=0;i<sp_S.Length;i++)
        {
            test_L.Add(sp_S[i]);
            all_SpritName += sp_S[i].name;
        }
        print("正确的图片名称构成的顺序" + all_SpritName);
        for (int i = test_L.Count-1; i >=0 ; i--)
        {//根据图片集，实例化按钮
            GameObject instan_Btn = Instantiate(btn_Gm) as GameObject;
            Button btn_ = instan_Btn.GetComponent<Button>();
            btn_.onClick.AddListener(delegate() { this.Btn_OnClick(instan_Btn.GetComponent<RectTransform>()); });//添加按钮事件
            int i1= Random.Range(0, test_L.Count);
            instan_Btn.name = test_L[i1].name;
            instan_Btn.GetComponent<Image>().sprite = test_L[i1];
            test_L.Remove(test_L[i1]);
            instan_Btn.transform.SetParent(bg_T);
            instan_Btn.GetComponent<RectTransform>().localScale = Vector3.one;
        }
        all_Btn = transform.FindChild("Img_Bg").GetComponentsInChildren<Button>();
        int random_Null = Random.Range(0, all_Btn.Length);//随机一个空图片位置
        all_Btn[random_Null].GetComponent<Image>().sprite = null;
        null_Img = all_Btn[random_Null].GetComponent<RectTransform>();
    }

    /// <summary>拼图按钮点击事件
    /// </summary>
    public void Btn_OnClick(RectTransform btn_Rect)
    {
        if (Vector2.Distance(btn_Rect.anchoredPosition,null_Img.anchoredPosition)==bg_Layout.cellSize.x+bg_Layout.spacing.x)
        {
            print("与空图片相近，点击图片的按钮的图片和空图片的Sprit图片  相互替换");
            Sprite huanCun = btn_Rect.GetComponent<Image>().sprite;
            btn_Rect.GetComponent<Image>().sprite = null_Img.GetComponent<Image>().sprite;
            btn_Rect.gameObject.name = null_Img.gameObject.name;
            null_Img.GetComponent<Image>().sprite = huanCun;
            null_Img.gameObject.name = huanCun.name;
            null_Img = btn_Rect;
            BtnName_All();
        }
    }

    /// <summary>遍历拼图块的名字， 累加，判断是否达成拼图完成
    /// </summary>
    void BtnName_All()
    {
        all_BtnSpriteName = "";
        all_Btn =bg_T.GetComponentsInChildren<Button>();
        for (int i=0;i<all_Btn.Length;i++)
        {
            all_BtnSpriteName += all_Btn[i].gameObject.name;
        }
        if (all_BtnSpriteName == all_SpritName)
        {
            Debug.Log("拼图完成！！！！！");
            Debug.LogError("游戏结束！此报错可以删除");
        }
    }
}
