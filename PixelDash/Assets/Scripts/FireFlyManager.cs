using UnityEngine;
using System.Collections;

public class FireFlyManager : MonoBehaviour
{

    public GameObject fireflyProfab;
    public float CD = 3.2f;

    float tickTimer;
    void Start()
    {
        instatance = this.GetComponent<FireFlyManager>();
    }
    public void BeginGame()
    {
        RandomMode();
        CD = 3.2f;
        tickTimer = CD - 1;
    }
    public static FireFlyManager GetInstance()
    {
        if (instatance)
        {
            return instatance;
        }
        else
        {
            return null;
        }

    }
    static FireFlyManager instatance;
    // Update is called once per frame
    void Update()
    {
        if (GameManager.currentState != GameManager.GameState.Start)
        {
            return;
        }
        if (isChangeMode)
        {
            return;
        }
        tickTimer += Time.deltaTime;
        if (tickTimer > CD)
        {
            CreatFirefly();
        }
    }
    GameObject go;

    bool isChangeMode;

    int count;
    int mode;
    Vector3[] normal;
    int rotateSpeedDir = 0;//模式三是否改变旋转方向
    float rotateSpeedAdd = 0;//模式三随分数提高增加旋转速度上限值220（min150）
    int mode1Style;//模式一和模式二的类型-0：正常  1-对称   2-两排
    int isFired;//模式三是否开启幻影

    public void RandomMode()
    {
        isChangeMode = true;
        count = 0;
        mode = Random.Range(1, 4);
        switch (mode)
        {
            case 1:
                count = Random.Range(1 + Random.Range(1, 5 + GameManager.score / 7), 3 + Random.Range(0, 5 + GameManager.score / 3));
                count = Mathf.Clamp(count, 1, 17);
                if (count >= 7)
                {
                    if (count % 2 == 1)
                    {
                        count++;
                    }
                    mode1Style = Random.Range(0, 3);
                }
                else
                {
                    mode1Style = Random.Range(0, 2);
                    if (mode1Style == 1)
                    {
                        if (count % 2 == 1)
                        {
                            count++;
                        }
                    }
                }

                break;
            case 2:
                count = Random.Range(2 + Random.Range(1, 6 + GameManager.score / 4), 4 + Random.Range(0, 6 + GameManager.score / 4));
                count = Mathf.Clamp(count, 2, 17);
                if (count >= 7)
                {
                    mode1Style = Random.Range(0, 3);
                    if (count % 2 == 1)
                    {
                        count++;
                    }
                }
                else
                {
                    mode1Style = Random.Range(0, 2);
                    if (mode1Style == 1)
                    {
                        if (count % 2 == 1)
                        {
                            count++;
                        }
                    }
                }

                break;
            case 3:
                count = Random.Range(1 + Random.Range(1, 5 + GameManager.score / 5), 4 + Random.Range(0, 6 + GameManager.score / 5));
                count = Mathf.Clamp(count, 2, 13);
                rotateSpeedDir = Random.Range(1, 3);
                rotateSpeedAdd = Mathf.Clamp((GameManager.score / 10) * 10, 0, 70);
                if (count > 6)
                {
                    isFired = 1;
                }
                else
                {
                    isFired = Random.Range(0, 2);
                }

                break;
            case 4: break;
        }
        GetNormal();
        //tickTimer = 3;
        CD = 3.2f - (GameManager.score / 3) * 0.1f;
        CD = Mathf.Clamp(CD, 1.1f, 3.2f);
        if (mode == 2)
        {
            CD = Mathf.Clamp(CD, 1.3f, 5f);
            if (mode1Style == 2)
            {
                CD = Mathf.Clamp(CD, 1.6f, 5f);
            }
        }
    }
    float angle;
    void GetNormal()
    {
        normal = new Vector3[count];

        Vector3 v = transform.right * 800;
        normal[0] = v;
        Quaternion r = transform.rotation;

        #region "MODE1"
        if (mode == 1)
        {
            angle = 4;
            if (mode1Style == 0)
            {
                for (int i = 1; i < count; i++)
                {
                    Quaternion q = Quaternion.Euler(r.eulerAngles.x, r.eulerAngles.y, r.eulerAngles.z + (angle * i)); ///求出第i个点的旋转角度
                    v = new Vector3(10, 40, 0) + (q * Vector3.right) * 800;///该点的坐标
                    if (i >= 6)
                    {
                        v = -v;
                    }
                    normal[i] = v;
                }

            }
            else if (mode1Style == 1)
            {
                //对称
                for (int i = 1; i < count; i++)
                {
                    Quaternion q;
                    if (i < count / 2)
                    {
                        q = Quaternion.Euler(r.eulerAngles.x, r.eulerAngles.y, r.eulerAngles.z + (angle * i)); ///求出第i个点的旋转角度
                        v = new Vector3(0, 10, 0) + (q * Vector3.right) * 800;///该点的坐标
                    }
                    else
                    {
                        q = Quaternion.Euler(r.eulerAngles.x, r.eulerAngles.y, r.eulerAngles.z + (angle * (i - count / 2))); ///求出第i个点的旋转角度
                        v = new Vector3(0, 10, 0) + (q * Vector3.left) * 800;///该点的坐标
                    }
                    normal[i] = v;

                }

            }
            else
            {
                //两排


                for (int i = 1; i < count; i++)
                {
                    Quaternion q;
                    if (i < count / 2)
                    {
                        q = Quaternion.Euler(r.eulerAngles.x, r.eulerAngles.y, r.eulerAngles.z + (angle * i)); ///求出第i个点的旋转角度
                        v = new Vector3(0, 10, 0) + (q * Vector3.right) * 800;///该点的坐标
                    }
                    else
                    {
                        q = Quaternion.Euler(r.eulerAngles.x, r.eulerAngles.y, r.eulerAngles.z + (angle * (i - count / 2))); ///求出第i个点的旋转角度
                        v = new Vector3(0, 10, 0) + (q * Vector3.right) * 800;///该点的坐标
                    }
                    normal[i] = v;
                }

            }
            isChangeMode = false;
            return;
        }
        #endregion


        if (mode == 3)
        {
            for (int i = 0; i < count; i++)
            {
                v = Vector3.zero;
                normal[i] = v;
            }
            isChangeMode = false;
            return;
        }

        if (mode == 2)
        {
            angle = 4;
            if (mode1Style == 0)
            {
                for (int i = 1; i < count; i++)
                {
                    Quaternion q = Quaternion.Euler(r.eulerAngles.x, r.eulerAngles.y, r.eulerAngles.z + (angle * i)); ///求出第i个点的旋转角度
                    v = new Vector3(0, 10, 0) + (q * Vector3.right) * 800;///该点的坐标
                    if (i >= 6)
                    {
                        v = -v;
                    }
                    normal[i] = v;
                }
            }
            else if (mode1Style == 1)
            {
                for (int i = 1; i < count; i++)
                {
                    Quaternion q;
                    if (i < count / 2)
                    {
                        q = Quaternion.Euler(r.eulerAngles.x, r.eulerAngles.y, r.eulerAngles.z + (angle * i)); ///求出第i个点的旋转角度
                        v = new Vector3(0, 10, 0) + (q * Vector3.right) * 800;///该点的坐标
                    }
                    else
                    {
                        q = Quaternion.Euler(r.eulerAngles.x, r.eulerAngles.y, r.eulerAngles.z + (angle * (i - count / 2))); ///求出第i个点的旋转角度
                        v = new Vector3(0, 10, 0) + (q * Vector3.left) * 800;///该点的坐标
                    }
                    normal[i] = v;
                }

            }
            else
            {

                for (int i = 1; i < count; i++)
                {
                    Quaternion q;
                    if (i < count / 2)
                    {
                        q = Quaternion.Euler(r.eulerAngles.x, r.eulerAngles.y, r.eulerAngles.z + (angle * i)); ///求出第i个点的旋转角度
                        v = new Vector3(0, 10, 0) + (q * Vector3.right) * 800;///该点的坐标
                    }
                    else
                    {
                        q = Quaternion.Euler(r.eulerAngles.x, r.eulerAngles.y, r.eulerAngles.z + (angle * (i - count / 2))); ///求出第i个点的旋转角度
                        v = new Vector3(0, 10, 0) + (q * Vector3.right) * 800;///该点的坐标
                    }
                    normal[i] = v;
                }

            }

        }
        isChangeMode = false;

    }
    void CreatFirefly()
    {
        if (count == 0)
        {
            return;
        }
        tickTimer = 0;
        switch (mode)
        {
            case 1:

                #region "case1"
                if (mode1Style <= 1)
                {
                    for (int i = 0; i < count; i++)
                    {
                        go = Instantiate(fireflyProfab);
                        go.transform.SetParent(GameManager.GetParent());
                        go.transform.localScale = Vector3.one;
                        go.transform.localPosition = new Vector3(0, 10, 0);
                        go.GetComponent<FireFlyModeOne>().InitFireFly(2.5f, normal[i], 0);
                    }
                }
                else
                {
                    for (int i = 0; i < count; i++)
                    {
                        go = Instantiate(fireflyProfab);
                        go.transform.SetParent(GameManager.GetParent());
                        go.transform.localScale = Vector3.one;
                        go.transform.localPosition = new Vector3(0, 10, 0);
                        if (i >= count / 2)
                        {
                            go.GetComponent<FireFlyModeOne>().InitFireFly(2.5f, normal[i], 0.1f);
                        }
                        else
                        {
                            go.GetComponent<FireFlyModeOne>().InitFireFly(2.5f, normal[i], 0);
                        }

                    }

                }
                #endregion
                break;
            case 2:

                if (mode1Style <= 1)
                {
                    for (int i = 0; i < count; i++)
                    {
                        go = Instantiate(fireflyProfab);
                        go.transform.SetParent(GameManager.GetParent());
                        go.transform.localScale = Vector3.one;
                        go.transform.localPosition = new Vector3(0, 10, 0);
                        go.GetComponent<FireFlyModeTwo>().InitFireFly(2.5f, normal[i]);
                    }
                }
                else
                {
                    //两排
                    for (int i = 0; i < count; i++)
                    {
                        go = Instantiate(fireflyProfab);
                        go.transform.SetParent(GameManager.GetParent());
                        go.transform.localScale = Vector3.one;
                        go.transform.localPosition = new Vector3(0, 10, 0);
                        if (i >= count / 2)
                        {
                            go.GetComponent<FireFlyModeTwo>().InitFireFly(2.5f, normal[i], 0.65f);
                        }
                        else
                        {
                            go.GetComponent<FireFlyModeTwo>().InitFireFly(2.5f, normal[i], 0);
                        }

                    }
                }
                break;
            case 3:

                if (isFired == 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        go = Instantiate(fireflyProfab);
                        go.transform.SetParent(GameManager.GetParent());
                        go.transform.localScale = Vector3.one;
                        go.GetComponent<FireModeThree>().InitFireFly(rotateSpeedDir, 0.4f - 0.05f * i, rotateSpeedAdd, 0);
                    }
                }
                else
                {
                    for (int i = 0; i < count / 2; i++)
                    {
                        go = Instantiate(fireflyProfab);
                        go.transform.SetParent(GameManager.GetParent());
                        go.transform.localScale = Vector3.one;
                        go.GetComponent<FireModeThree>().InitFireFly(rotateSpeedDir, 0.4f - 0.05f * i, rotateSpeedAdd, 1);
                    }

                }

                break;
            case 4: break;
        }

    }
}
