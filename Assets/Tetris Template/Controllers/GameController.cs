using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public Transform blockHolder;
    public GameObject adWatchPanelCopy;
    public static GameObject adWatchPanel;
    public GameObject controllsPanel;
    public GameObject gamepadCross;
    public GameObject soundCross;

    public GameObject bonusScoreObj;
    private float bonusScorePosX = -175;

    public static int NumberOfFails = 0;
    public static bool isGameOverUiActive = false;
    public static bool clearRowsAdWatched = false;
    public static bool isGamePaused = true;

    public static Color32 orangeNoAlpha = new Color32(237, 149, 74, 0);
    public static Color32 orange = new Color32(237, 149, 74, 255);
    public const string GAMEPAD_ACTIVE_KEY = "GamepadActive";
    public const string SOUND_ACTIVE_KEY = "SoundActive";
    public bool isGamepadActive = false;
    public int comboNum = 0;

    public System.Random random = new System.Random();

    private void Awake()
    {
        Instance = this;

        LoadPreferences();

        adWatchPanel = adWatchPanelCopy;

        adWatchPanel.GetComponent<Button>().onClick.AddListener(delegate { WatchAdAndGetReward(); });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            StartCoroutine(Instance.IncreaseCombo());
        }
    }

    private void LoadPreferences()
    {
        isGamepadActive = PlayerPrefs.GetInt(GAMEPAD_ACTIVE_KEY, 0) == 1;
        ShowOrHideGamepadSelection();
        bool soundActive = PlayerPrefs.GetInt(SOUND_ACTIVE_KEY, 0) == 1;
        if (!soundActive)
        {
            soundCross.SetActive(true);
            AudioListener.volume = 0f;
        }
    }

    public void OnGameControllerChange()
    {
        isGamepadActive = !isGamepadActive;
        ShowOrHideGamepadSelection();
        PlayerPrefs.SetInt(GAMEPAD_ACTIVE_KEY, isGamepadActive ? 1 : 0);
    }

    public void OnSoundPreferenceChange(bool isActive)
    {
        PlayerPrefs.SetInt(SOUND_ACTIVE_KEY, isActive ? 1 : 0);
    }

    private void ShowOrHideGamepadSelection()
    {
        if (isGamepadActive == true)
            gamepadCross.SetActive(false);
        else
            gamepadCross.SetActive(true);
    }

    private IEnumerator ClearLines()
    {
        StartCoroutine(DeleteRow(0));
        yield return new WaitForSeconds(0.4f);
        StartCoroutine(DeleteRow(0));
        yield return new WaitForSeconds(0.4f);
        StartCoroutine(DeleteRow(0));
        yield return new WaitForSeconds(0.4f);

        bool spawnNewShape = true;

        foreach (Transform child in blockHolder)
        {
            if (child.GetComponent<TetrisShape>().isActiveAndEnabled)
                spawnNewShape = false;
        }

        if (spawnNewShape == true)
            Managers.Spawner.Spawn();
    }

    private IEnumerator DeleteRow(int k)
    {
        // Destroy the gameobjects in the line
        for (int x = 0; x < 10; ++x)
        {
            if (Managers.Grid.gameGridcol[x].row[k] != null)
            {
                Destroy(Managers.Grid.gameGridcol[x].row[k].gameObject);
                Managers.Grid.gameGridcol[x].row[k] = null;
            }
        }

        // Decrease rows
        for (int y = k; y < 20; ++y)
        {
            bool isEmptry = true;

            for (int x = 0; x < 10; x++)
            {
                if (Managers.Grid.gameGridcol[x].row[y] != null || Managers.Grid.gameGridcol[x].row[y+1] != null)
                {
                    isEmptry = false;
                }
            }

            if (isEmptry == false)
                Managers.Grid.DecreaseRow(y);
            else
                y = 20;
        }
            
        Managers.Audio.PlayLineClearSound();

        foreach (Transform t in Managers.Game.blockHolder)
        {
            if (t.childCount <= 1)
            {
                Destroy(t.gameObject);
            }
        }

        yield break;
    }

    // Show the ad watch panel
    public static IEnumerator ShowAdPanel()
    {
        if (clearRowsAdWatched == false)
        {
            yield return new WaitForSeconds(0.15f);
            adWatchPanel.SetActive(true);
            Color32 textColor = new Color32(255, 255, 255, 255);
            adWatchPanel.GetComponent<Image>().DOColor(orange, 0.7f);
            adWatchPanel.transform.GetChild(0).GetComponent<Text>().DOColor(textColor, 0.7f);
        }
    }

    // Close the ad watch panel
    public static void CloseAdWatchPanel()
    {
        adWatchPanel.SetActive(false);
        adWatchPanel.GetComponent<Image>().color = new Color32(237, 149, 74, 0);
        adWatchPanel.transform.GetChild(0).GetComponent<Text>().color = new Color32(255, 255, 255, 0);
    }

    // Continue the game and watch an ad, when finished, clear the bottom 3 rows
    public void WatchAdAndGetReward()
    {
        // Play an ad
        Managers.Adv.ShowRewardedAd();

        // Unpause the game
        isGamePaused = false;
    }

    public IEnumerator OnRewardAdFinish()
    {
        clearRowsAdWatched = true;

        // Continue the game
        Managers.Game.SetState(typeof(GamePlayState));
        CloseAdWatchPanel();
        Managers.Game.currentShape.movementController.StopInstantFall();
        yield return new WaitForSeconds(1f);
        // Clear the bottom 3 lines
        StartCoroutine(ClearLines());
    }

    #region Gamepad Controlls
    public void OnLeftArrowClick()
    {
        Managers.Game.currentShape.movementController.MoveHorizontal(Vector2.left);
    }

    public void OnRightArrowClick()
    {
        Managers.Game.currentShape.movementController.MoveHorizontal(Vector2.right);
    }

    public void OnDownArrowClick()
    {
        if (Managers.Game.currentShape != null)
        {
            Managers.Game.currentShape.movementController.InstantFall();
        }
    }

    public void OnUpArrowClick()
    {
        if (Managers.Game.currentShape != null)
        {
            Managers.Game.currentShape.movementController.StopInstantFall();
        }
    }
    #endregion /Gamepad Controlls

    public static void OnGameRestartClick()
    {
        clearRowsAdWatched = false;
        isGamePaused = false;
    }

    public static void OnHomeScreenClick()
    {
        clearRowsAdWatched = false;
        isGamePaused = true;
    }

    public void OnGameContinueClick()
    {
        isGamePaused = false;
        StartCoroutine(StopInstantFallOnContinue());
    }

    private IEnumerator StopInstantFallOnContinue()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(0.04f);
        Managers.Game.currentShape.movementController.StopInstantFall();
    }

    public static void OnGamePauseClick()
    {
        isGamePaused = true;
    }

    #region Manage Combos

    public IEnumerator IncreaseCombo()
    {
        comboNum++;

        if (comboNum == 2)
        {
            bonusScoreObj.GetComponent<Text>().text = "+ 100";
            ShowBonusScoreText();
            AnimateAdditionalScoreText();
        }
        else if (comboNum > 2)
        {
            bonusScoreObj.GetComponent<Text>().text = "+ " + (comboNum - 1) * 100;
            AnimateAdditionalScoreText();
        }

        int prevCombo = comboNum;

        yield return new WaitForSeconds(1);

        // If the combo remained the same during the wait time
        //  then reset it
        if (prevCombo == comboNum)
        {
            if (comboNum == 1)
            {
                comboNum = 0;
                yield break;
            }

            int bonus = (comboNum - 1) * 100;

            for (int i = bonus; i > 0; i-=25)
            {
                Managers.Score.OnScore(25);
                bonusScoreObj.GetComponent<Text>().text = "+ " + (i - 25);
                yield return new WaitForSeconds(0.1f);
            }

            comboNum = 0;
            HideBonusScoreText();
        }
    }

    private void ShowBonusScoreText()
    {
        int scoreLen = StatsController.Instance.highScore.ToString().Length;
        bonusScoreObj.GetComponent<RectTransform>().anchoredPosition =
            new Vector3(bonusScorePosX + (scoreLen - 3) * 20, -175, 0);

        bonusScoreObj.GetComponent<Text>().DOColor(orange, 0.3f).SetEase(Ease.InSine);
    }

    private void HideBonusScoreText()
    {
        bonusScoreObj.GetComponent<Text>().DOColor(orangeNoAlpha, 0.3f).SetEase(Ease.OutSine);
    }

    private void AnimateAdditionalScoreText()
    {
        bonusScoreObj.GetComponent<RectTransform>().DOScale(1.1f, 0.4f)
            .OnComplete(delegate 
            {
                bonusScoreObj.GetComponent<RectTransform>().DOScale(1f, 0.4f)
                .SetEase(Ease.OutSine);
            })
            .SetEase(Ease.InSine);
    }

    #endregion /Manage Combos
}