using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BtnClickManager : MonoBehaviour
{

    public Text txtCenter;
    public Image bgCenter;
    public Image bgGame;
    public Image bgCover;
    public Text txtBottom;
    public Image btnClose;
    public Image btnHelp;
    public Image btnNext;
    public Image btnFore;

    public GameObject bloodController;
    public RectTransform bloodCover;


    public Transform parent;
    public RectTransform minRadius;
    public RectTransform maxRadius;

    public float[] GetRadius()
    {
        float min = Vector2.Distance(minRadius.position, bgCenter.rectTransform.position);
        float max = Vector2.Distance(maxRadius.position, bgCenter.rectTransform.position);
        float[] radius = new float[2] { min, max };
        return radius;
    }

    public void InitLoadingState()
    {
        bgCenter.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 170);
        bgCenter.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 170);
        txtCenter.text = "PIXEL\r\nDASH";
        btnClose.gameObject.SetActive(false);
        btnHelp.gameObject.SetActive(false);
        btnNext.gameObject.SetActive(false);
        btnFore.gameObject.SetActive(false);
        GameManager.RegisterGAMEBG(bgGame, txtCenter, bgCover);
        GameManager.RegisterCenterBG(bgCenter.rectTransform, parent, minRadius);
        bloodController.SetActive(false);
    }

    public void CenterClick()
    {
        if (GameManager.currentState == GameManager.GameState.Loading)
        {
            bgCenter.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 410);
            bgCenter.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 410);
            GameManager.currentState = GameManager.GameState.ControleMode;
            SetControllModeTXT(GameManager.controllMode);
            btnClose.gameObject.SetActive(true);
            btnHelp.gameObject.SetActive(true);
            btnNext.gameObject.SetActive(true);
            btnFore.gameObject.SetActive(true);
            bloodController.SetActive(false);
        }
        else if (GameManager.currentState == GameManager.GameState.ControleMode)
        {
            GameManager.currentState = GameManager.GameState.Difficulty;
            SetDifficultyTXT(GameManager.difficulty);
        }
        else if (GameManager.currentState == GameManager.GameState.Difficulty)
        {
            GameManager.currentState = GameManager.GameState.Ready;
            bgCenter.GetComponent<Button>().enabled = false;
            bgCenter.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 170);
            bgCenter.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 170);
            txtCenter.text = "SCORE\r\n<size=55><i>0</i></size>";
            btnHelp.gameObject.SetActive(false);
            btnNext.gameObject.SetActive(false);
            btnFore.gameObject.SetActive(false);
            txtBottom.gameObject.SetActive(false);
            bloodController.SetActive(true);
            GameManager.BeginGame();
            //#warning "SonicBoy------Please delete the following code!!!!!!"
            //            //以下两行代码仅在测试中间按钮点击循环时使用，其他任何时候都应该注释掉下列代码！！
            //            //GameManager.currentState = GameManager.GameState.Over;
            //            //bgCenter.GetComponent<Button>().enabled = true;
        }
        else if (GameManager.currentState == GameManager.GameState.Over)
        {
            GameManager.currentState = GameManager.GameState.Ready;
            bgCenter.GetComponent<Button>().enabled = false;
            bgCenter.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 170);
            bgCenter.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 170);
            txtCenter.text = "SCORE\r\n<size=55><i>0</i></size>";
            btnHelp.gameObject.SetActive(false);
            btnNext.gameObject.SetActive(false);
            btnFore.gameObject.SetActive(false);
            txtBottom.gameObject.SetActive(false);
            bloodController.SetActive(true);
            GameManager.BeginGame();
        }
        GameManager.ChangeBGColor();
        GameManager.PlayAudioOK();
    }

    public void BackClick()
    {
        GameManager.CloseBGM();
        GameManager.DestorySquare();
        if (GameManager.currentState == GameManager.GameState.ControleMode)
        {
            GameManager.currentState = GameManager.GameState.Loading;
            InitLoadingState();
        }
        else if (GameManager.currentState == GameManager.GameState.Difficulty)
        {
            GameManager.currentState = GameManager.GameState.ControleMode;
            SetControllModeTXT(GameManager.controllMode);
        }
        else if (GameManager.currentState == GameManager.GameState.Over || GameManager.currentState == GameManager.GameState.Start)
        {
            bgCenter.GetComponent<Button>().enabled = true;
            GameManager.currentState = GameManager.GameState.Difficulty;
            bgCenter.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 410);
            bgCenter.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 410);
            SetDifficultyTXT(GameManager.difficulty);
            btnHelp.gameObject.SetActive(true);
            btnNext.gameObject.SetActive(true);
            btnFore.gameObject.SetActive(true);
            txtBottom.gameObject.SetActive(true);
            bloodController.SetActive(false);
        }

        GameManager.ChangeBGColor();
        GameManager.PlayAudioError();
    }

    public void ShowGameOverUI()
    {
        bgCenter.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 410);
        bgCenter.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 410);
        txtCenter.text = " <size=55>GAME OVER</size>\r\n\r\n<SIZE=30>YOUR SCORE</SIZE>\r\n" + GameManager.score + "\r\n\r\n<SIZE=25>CLICK CENTER TO RESTART</SIZE>";
        bgCenter.GetComponent<Button>().enabled = true;
    }

    void SetControllModeTXT(GameManager.GameControllMode gcm)
    {
        switch (gcm)
        {
            case GameManager.GameControllMode.Auto:
                txtCenter.text = "CONTROL MODE\r\n<size=55><i>AUTO</i></size>\r\nTOUCH=RADIUS";
                break;
            case GameManager.GameControllMode.Manual:
                txtCenter.text = "CONTROL MODE\r\n<size=55><i>MANUAL</i></size>\r\nRIGHT=RADIUS++\r\nLEFT=RADIUS--";
                break;
            case GameManager.GameControllMode.Full:
                txtCenter.text = "CONTROL MODE\r\n<size=55><i>FULL</i></size>\r\nRIGHT=RADIUS++\r\nLEFT=DIRECTION";
                break;
            case GameManager.GameControllMode.FullAdd:
                txtCenter.text = "CONTROL MODE\r\n<size=55><i>FULL+</i></size>\r\nRIGHT=RADIUS++\r\nLEFT=RADIUS--\r\nBOTH=DIRECTION";
                break;
        }

    }

    void SetDifficultyTXT(GameManager.GameDifficulty diff)
    {
        switch (diff)
        {
            case GameManager.GameDifficulty.Normal:
                txtCenter.text = "DIFFUILTY\r\n<size=55><i>NORMAL</i></size>";
                break;
            case GameManager.GameDifficulty.Difficulty:
                txtCenter.text = "DIFFUILTY\r\n<size=55><i>DIFFICULTY</i></size>";
                break;
            default:
                break;
        }

    }

    public void SelectClick(int code)
    {
        if (code == 2)
        {
            if (GameManager.currentState == GameManager.GameState.ControleMode)
            {
                if (GameManager.controllMode == GameManager.GameControllMode.Auto)
                {
                    GameManager.controllMode = GameManager.GameControllMode.FullAdd;
                }
                else if (GameManager.controllMode == GameManager.GameControllMode.Manual)
                {
                    GameManager.controllMode = GameManager.GameControllMode.Auto;
                }
                else if (GameManager.controllMode == GameManager.GameControllMode.Full)
                {
                    GameManager.controllMode = GameManager.GameControllMode.Manual;
                }
                else if (GameManager.controllMode == GameManager.GameControllMode.FullAdd)
                {
                    GameManager.controllMode = GameManager.GameControllMode.Full;
                }


                SetControllModeTXT(GameManager.controllMode);
            }
            else if (GameManager.currentState == GameManager.GameState.Difficulty)
            {
                if (GameManager.difficulty == GameManager.GameDifficulty.Normal)
                {
                    GameManager.difficulty = GameManager.GameDifficulty.Difficulty;
                }
                else
                {
                    GameManager.difficulty = GameManager.GameDifficulty.Normal;
                }

                SetDifficultyTXT(GameManager.difficulty);
            }
        }
        else
        {
            if (GameManager.currentState == GameManager.GameState.ControleMode)
            {
                if (GameManager.controllMode == GameManager.GameControllMode.Auto)
                {
                    GameManager.controllMode = GameManager.GameControllMode.Manual;
                }
                else if (GameManager.controllMode == GameManager.GameControllMode.Manual)
                {
                    GameManager.controllMode = GameManager.GameControllMode.Full;
                }
                else if (GameManager.controllMode == GameManager.GameControllMode.Full)
                {
                    GameManager.controllMode = GameManager.GameControllMode.FullAdd;
                }
                else if (GameManager.controllMode == GameManager.GameControllMode.FullAdd)
                {
                    GameManager.controllMode = GameManager.GameControllMode.Auto;
                }
                SetControllModeTXT(GameManager.controllMode);
            }
            else if (GameManager.currentState == GameManager.GameState.Difficulty)
            {
                if (GameManager.difficulty == GameManager.GameDifficulty.Normal)
                {
                    GameManager.difficulty = GameManager.GameDifficulty.Difficulty;
                }
                else
                {
                    GameManager.difficulty = GameManager.GameDifficulty.Normal;
                }

                SetDifficultyTXT(GameManager.difficulty);
            }

        }
        GameManager.ChangeBGColor();

    }

    public void LeftDown()
    {
        GameManager.isPressedLeft = true;
    }
    public void LeftUp()
    {
        GameManager.isPressedLeft = false;
    }
    public void RightDown()
    {
        GameManager.isPressedRight = true;
    }
    public void RightUp()
    {
        GameManager.isPressedRight = false;
    }

    public void ShowScore(int score)
    {

        txtCenter.text = "SCORE\r\n<size=55><i>" + score + "</i></size>";
    }


    public void ShowDamage()
    {
        bloodCover.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, GameManager.currentDamage * 128);
        bloodCover.GetComponent<Image>().color = new Color(255, 0, 0, (40 + GameManager.currentDamage * 15) / 255.0f);
    }


}
