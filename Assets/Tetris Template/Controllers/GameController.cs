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
    public GameObject gamepadStatusObj;

    public static int NumberOfFails = 0;
    public static bool isGameOverUiActive = false;
    public static bool clearRowsAdWatched = false;
    public static bool isGamePaused = false;

    public const string GAMEPAD_ACTIVE_KEY = "GamepadActive";
    public bool isGamepadActive = false;

    private void Awake()
    {
        Instance = this;

        LoadPreferences();

        adWatchPanel = adWatchPanelCopy;

        adWatchPanel.GetComponent<Button>().onClick.AddListener(delegate { WatchAdAndGetReward(); });
    }

    private void LoadPreferences()
    {
        isGamepadActive = PlayerPrefs.GetInt(GAMEPAD_ACTIVE_KEY, 0) == 1;
        ShowOrHideGamepadSelection();
    }

    public void OnGameControllerChange()
    {
        isGamepadActive = !isGamepadActive;
        ShowOrHideGamepadSelection();
        PlayerPrefs.SetInt(GAMEPAD_ACTIVE_KEY, isGamepadActive ? 1 : 0);
    }

    private void ShowOrHideGamepadSelection()
    {
        if (isGamepadActive == true)
            gamepadStatusObj.SetActive(false);
        else
            gamepadStatusObj.SetActive(true);
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
            Color32 buttonColor = new Color32(255, 255, 255, 200);
            Color32 textColor = new Color32(237, 149, 74, 255);
            adWatchPanel.GetComponent<Image>().DOColor(buttonColor, 0.5f);
            adWatchPanel.transform.GetChild(0).GetComponent<Text>().DOColor(textColor, 0.5f);
        }
    }

    // Close the ad watch panel
    public static void CloseAdWatchPanel()
    {
        adWatchPanel.SetActive(false);
        adWatchPanel.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        adWatchPanel.transform.GetChild(0).GetComponent<Text>().color = new Color32(237, 149, 74, 0);
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
        isGamePaused = false;
    }

    public static void OnGameContinueClick()
    {
        isGamePaused = false;
    }

    public static void OnGamePauseClick()
    {
        isGamePaused = true;
    }
}