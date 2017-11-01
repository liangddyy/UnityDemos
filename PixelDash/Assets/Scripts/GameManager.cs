using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public enum GameState
    {
        Loading,
        ControleMode,
        Difficulty,
        Ready,
        Start,
        Over
    }
    public enum GameControllMode
    {
        Auto,
        Manual,
        Full,
        FullAdd
    }
    public enum GameDifficulty
    {
        Normal,
        Difficulty
    }

    public Color[] BGColors;
    static Color[] sBgColors;
    public Sprite[] imgCovers;
    static Sprite[] sImgCovers;
    public static GameState currentState;
    public static GameControllMode controllMode;
    public static GameDifficulty difficulty;
    public FireFlyManager fireManager;
    static BtnClickManager clickManager;
    static SquareManager squareManager;

    static FireFlyManager sfireManager;

    static Image gameBG;
    static Image coverBG;
    static Text centerTXT;

    public GameObject playerProfab;
    static GameObject sPlayerProfab;

    static AudioManager musicManager;

    public static int score;
    // Use this for initialization
    void Start()
    {
        clickManager = GetComponent<BtnClickManager>();
        squareManager = GetComponent<SquareManager>();
        sfireManager = fireManager;
        musicManager = GetComponent<AudioManager>();

        currentState = GameState.Loading;
        controllMode = GameControllMode.Auto;
        difficulty = GameDifficulty.Normal;
        sBgColors = BGColors;
        sImgCovers = imgCovers;
        sPlayerProfab = playerProfab;
        clickManager.InitLoadingState();
        squareManager.InitSquareManager();
    }

    public static void RegisterGAMEBG(Image img, Text txt, Image bgcov)
    {
        gameBG = img;
        centerTXT = txt;
        coverBG = bgcov;
        ChangeBGColor();
    }
    //gameCenter游戏的中心点，所有旋转绕其进行
    //gameParent，新添加游戏对象的parent均为gameParent；
    static RectTransform gameCenter;
    static RectTransform playBornPos;
    static Transform gameParent;
    public static void RegisterCenterBG(RectTransform center, Transform parent, RectTransform playPos)
    {
        gameCenter = center;
        gameParent = parent;
        playBornPos = playPos;
    }

    public static RectTransform GetGameCnter()
    {
        return gameCenter;

    }
    static Color lastColor;
    static Sprite lastSprite;
    public static void ChangeBGColor()
    {
        if (gameBG && sBgColors.Length > 1)
        {
            do
            {
                lastColor = sBgColors[Random.Range(0, sBgColors.Length)];
            } while (lastColor == gameBG.color);

            gameBG.color = lastColor;
            centerTXT.color = lastColor;

            do
            {
                lastSprite = sImgCovers[Random.Range(0, sImgCovers.Length)];
            } while (lastSprite == coverBG.sprite);
            coverBG.sprite = lastSprite;



        }
    }

    public static float[] GetRadius()
    {
        return clickManager.GetRadius();
    }
    static GameObject player;
    public static void BeginGame()
    {
        currentDamage = 0;
        score = 0;
        clickManager.ShowDamage();

        //创建玩家
        if (!sPlayerProfab)
        {
            return;
        }
        if (player)
        {
            Destroy(player.gameObject);
            //清空分数,然后根据难度和控制模式选择加载不同的脚本
        }
        player = GameObject.Instantiate(sPlayerProfab);
        player.transform.SetParent(gameParent);
        player.transform.localPosition = playBornPos.localPosition;
        player.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        player.GetComponent<BasePlayer>().InitPlayer();

        switch (controllMode)
        {
            case GameControllMode.Auto:
                player.GetComponent<AutoPlayer>().enabled = true;
                player.GetComponent<AutoPlayer>().InitPlayerController(gameCenter);
                break;
            case GameControllMode.Manual:
                player.GetComponent<ManualPlayer>().enabled = true;
                player.GetComponent<ManualPlayer>().InitPlayerController(gameCenter);
                break;
            case GameControllMode.Full:
                player.GetComponent<FullPlayer>().enabled = true;
                player.GetComponent<FullPlayer>().InitPlayerController(gameCenter);
                break;
            case GameControllMode.FullAdd:
                player.GetComponent<FullAddPlayer>().enabled = true;
                player.GetComponent<FullAddPlayer>().InitPlayerController(gameCenter);
                break;
            default:
                player.GetComponent<AutoPlayer>().enabled = true;
                player.GetComponent<AutoPlayer>().InitPlayerController(gameCenter);
                break;
        }
        isReadyState = 0;
        score = 0;
        clickManager.ShowScore(score);
        squareManager.CreatSquare();
        FireFlyManager.GetInstance().BeginGame();
        musicManager.OpenBGM();
    }
    static int isReadyState;
    public static void isReady()
    {
        isReadyState++;
        if (isReadyState >= 2)
        {
            currentState = GameState.Start;
        }
    }

    public static Transform GetParent()
    {
        return gameParent;
    }



    /// <summary>
    /// Player撞击方块后，由方块调用
    /// </summary>
    public static void DashSquare()
    {
        sfireManager.RandomMode();
        ChangeBGColor();
        if (controllMode == GameControllMode.Auto || controllMode == GameControllMode.Manual)
        {
            //Auto和Manual需要自动改变方向
            BasePlayer.rotateSpeed = -BasePlayer.rotateSpeed;
        }
        //得分
        clickManager.ShowScore(++score);

        //更新DamageUI
        currentDamage--;
        if (currentDamage <= 0)
        {
            currentDamage = 0;
        }
        clickManager.ShowDamage();
    }
    /// <summary>
    /// 方块被撞击后要消失时调用，产生下一个方块
    /// </summary>
    public static void CreatSquare()
    {
        squareManager.CreatSquare();
    }
    public static void DestorySquare()
    {
        if (player)
        {
            Destroy(player.gameObject);
        }
        squareManager.DestroySquare();
    }
    public static bool isPressedLeft;
    public static bool isPressedRight;

    public static int currentDamage;
    public static void PlayDashed()
    {
        currentDamage++;
        if (currentDamage >= 10)
        {
            currentState = GameState.Over;
            Destroy(player.gameObject);
            clickManager.ShowGameOverUI();
            musicManager.CloseBGM();
        }
        clickManager.ShowDamage();
    }

    public static void PlayAudioOK()
    {
        musicManager.PlayAudioOK();
    }
    public static void PlayAudioError()
    {
        musicManager.PlayAudioError();

    }
    public static void CloseBGM()
    {
        musicManager.CloseBGM();

    }
    public static void GameOver()
    {


    }

}
