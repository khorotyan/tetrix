using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Transform blockHolder;
    public GameObject adWatchPanelCopy;
    public static GameObject adWatchPanel;

    public static int NumberOfFails = 0;
    public static bool isGameOverUiActive = false;

    private void Awake()
    {
        adWatchPanel = adWatchPanelCopy;

        adWatchPanel.GetComponent<Button>().onClick.AddListener(delegate { WatchAdAndGetReward(); });
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
        yield return new WaitForSeconds(1.3f);
        adWatchPanel.SetActive(true); 
    }

    // Close the ad watch panel
    public static void CloseAdWatchPanel()
    {
        adWatchPanel.SetActive(false);
    }

    // Continue the game and watch an ad, when finished, clear the bottom 3 rows
    public void WatchAdAndGetReward()
    {
        // Play an ad
        Managers.Adv.ShowRewardedAd();
    }

    public IEnumerator OnRewardAdFinish()
    {
        // Continue the game
        Managers.Game.SetState(typeof(GamePlayState));
        CloseAdWatchPanel();
        yield return new WaitForSeconds(1f);

        // Clear the bottom 3 lines
        StartCoroutine(ClearLines());
    }
}