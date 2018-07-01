using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PlayGamesController : MonoBehaviour
{
    private void Start()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();

        SignIn();
    }

    private void SignIn()
    {
        Social.localUser.Authenticate((bool success) => {
            // Handle success or failure
        });
    }

    #region Achievements
    public static void UnlockAchievement(string achievementId)
    {
        Social.ReportProgress(achievementId, 100.0f, (bool success) => { });
    }

    public static void IncrementAchievement(string achievementId, int stepsToIncrement)
    {
        PlayGamesPlatform.Instance.IncrementAchievement(achievementId, stepsToIncrement, (bool success) => { });
    }

    public static void ShowAchievementsUI()
    {
        Social.ShowAchievementsUI();
    }
    #endregion /Achievements

    #region Leaderboard
    public static void AddScoreToLeaderboard(string leaderboardId, int score)
    {
        Social.ReportScore(score, leaderboardId, (bool success) => { });
    }

    public static void ShowLeaderboardUI()
    {
        Social.ShowLeaderboardUI();
    }
    #endregion /Leaderboard
}
