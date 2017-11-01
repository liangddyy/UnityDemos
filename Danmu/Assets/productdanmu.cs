using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;

public class productdanmu : MonoBehaviour
{
    public GameObject textsdanmu, Danmu_parents; //弹幕预设物体和其父物体

    private GameObject texts; //生成的弹幕物体
    private Queue<GameObject> texts_queue = new Queue<GameObject>(); //弹幕物体的队列  
    private float production_timer; //生成弹幕的时间间隔

    private int[] numb = new int[] {39, 139, 239, 339}; //四行弹幕的位置

    private int Row_index; //弹幕行数变化
    private int a; ////弹幕行数变化中间变量（桥的作用）

    private Vector3 textpositon; //生成弹幕位置
    private Quaternion textrotation; //生产弹幕角度
    private string content; //弹幕文字内容


    void Start()
    {
        Row_index = numb[0]; //默认弹幕生产在第一行
        textpositon = new Vector3(96, Row_index, 0); //弹幕默认位置
        textrotation.eulerAngles = new Vector3(0, 0, 0);
        production_timer = 2f;
    }


    void Update()
    {
        production_timer -= Time.deltaTime;

        if (production_timer <= 0f)
        {
            int i = UnityEngine.Random.Range(0, DanMuStrings.Length); //弹幕的随机内容
            content = DanMuStrings[i];

            createDanMuEntity(); //调用生成弹幕方法

            production_timer = 2f; //每隔2秒生成一个弹幕
        }

        #region 判断应该在第几行

        foreach (GameObject tex in texts_queue.ToArray()) //判断每一行是否满了
        {
            if (tex.transform.localPosition.y == numb[0]) //第一行
            {
                a = 2;
            }
            else if (tex.transform.localPosition.y == numb[1]) //第二行
            {
                a = 3;
            }
            else if (tex.transform.localPosition.y == numb[2]) //第三行
            {
                a = 4;
            }
            else if (tex.transform.localPosition.y == numb[3]) //第4行
            {
                a = 1;
            }
        }

        switch (a)
        {
            case 1:
                Row_index = numb[0];
                break;
            case 2:
                Row_index = numb[1];
                break;
            case 3:
                Row_index = numb[2];
                break;
            case 4:
                Row_index = numb[3];
                break;
            default:
                Debug.Log("没有满足任何一行");
                break;
        }

        #endregion

        if (texts_queue.Count > 0) //退出队列方法一（节省资源开销）
        {
            GameObject go = texts_queue.Peek();

            if (go.transform.localPosition.x < -999) //当弹幕位置移出屏幕
            {
                texts_queue.Dequeue(); //移出队列
                Destroy(go); //销毁弹幕

                Debug.Log("save===" + texts_queue.Count);
            }
        }
    }

    [HideInInspector]

    #region 弹幕内容（测试用）

    public string[] DanMuStrings =
    {
        "这个剧情也太雷人了吧！",
        "还是好莱坞的电影经典啊，这个太次了还是好莱坞的电影经典啊，这个太次了",
        "是电锯惊魂的主角，尼玛",
        "这个游戏还是很良心的么",
        "卧槽还要花钱，这一关也太难卧槽还要花钱，这一关也太难了卧槽还要花钱，这一关也太难了卧槽还要花钱，这一关也太难了了",
        "这个游戏好棒偶",
        "全是傻逼",
        "求约：13122785566",
        "最近好寂寞啊，还是这个游戏好啊是胸再大点就更是胸再大点就更是胸再大点就更",
        "难道玩游戏还能撸",
        "办证：010 - 888888",
        "为什么女主角没有死？",
        "好帅呦，你这个娘们儿",
        "欠揍啊，东北人不知道啊",
        "我去都是什么人啊，请文明用语还是好莱坞的电影经典啊，这个太次了是胸再大点就更",
        "这个还是不错的",
        "要是胸再大点就更好了",
        "这个游戏必须顶啊",
        "还是好莱坞的电影经典啊，这个太次了还是好莱坞的电影经典啊，这个太次了怎么没有日本动作爱情片中的角色呢？",
        "好吧，这也是醉了！",
        "他只想做一个安静的美男子！"
    };

    #endregion

    public void createDanMuEntity() //生成弹幕和移动弹幕
    {
        texts = (GameObject) (Instantiate(textsdanmu, textpositon, textrotation)); //生成弹幕
        if (texts != null)
        {
            texts.transform.SetParent(Danmu_parents.transform); //设置父物体

            // texts.transform.parent = Danmu_parents.transform;//设置父物体的另种方法

            texts.transform.localScale = new Vector3(1, 1, 1);
            textrotation.eulerAngles = new Vector3(0, 0, 0);
            texts.transform.localRotation = textrotation;

            texts.transform.localPosition = new Vector3(290, Row_index, 0); //--弹幕移动的起始位置X           

            texts.transform.GetComponent<Text>().text = content; //弹幕内容               


            if (texts.GetComponent<DOTweenAnimation>() == null)
                texts.AddComponent<DOTweenAnimation>(); //添加DOTween插件
            texts.transform.DOLocalMoveX(-1200, 20); //移动弹幕（20秒移动到-1200的位置）
        }

        texts_queue.Enqueue(texts); //生成的弹幕添加到队列
    }
}