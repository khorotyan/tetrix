using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class StatsController : MonoBehaviour
{
    public static StatsController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

        LoadPlayerStats();
    }

    public const string HIGH_SCORE_KEY = "HighScore";
    public const string TOTAL_SCORE_KEY = "TotalScore";
    public const string TIME_SPENT_KEY = "TimeSpent";
    public const string NUMBER_OF_GAMES_KEY = "NumberOfGames";

    public int highScore = 0;
    public int totalScore = 0;
    public float timeSpent = 0;
    public int numberOfGames = 0;

    private void LoadPlayerStats()
    {
        highScore = PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);
        totalScore = PlayerPrefs.GetInt(TOTAL_SCORE_KEY, 0);
        timeSpent = PlayerPrefs.GetFloat(TIME_SPENT_KEY, 0);
        numberOfGames = PlayerPrefs.GetInt(NUMBER_OF_GAMES_KEY, 0);
    }

    private void SavePlayerStats()
    {
        PlayerPrefs.SetInt(HIGH_SCORE_KEY, highScore);
        PlayerPrefs.SetInt(TOTAL_SCORE_KEY, totalScore);
        PlayerPrefs.SetFloat(TIME_SPENT_KEY, timeSpent);
        PlayerPrefs.SetInt(NUMBER_OF_GAMES_KEY, numberOfGames);
    }

    public void OnLeaderboardClick()
    {
        PlayGamesController.ShowLeaderboardUI();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            SavePlayerStats();
        }
    }

    private void OnApplicationQuit()
    {
        SavePlayerStats();
    }
}
