using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour {

    public Text highScore;
    public Text totalScore;
    public Text timeSpent;
    public Text numberOfGames;

    public void ClearStats()
    {
        //StatsController.Instance.ClearStats();
        RefreshText();
    }

    void OnEnable()
    {
        RefreshText();
    }

    void RefreshText()
    {
        highScore.text = StatsController.Instance.highScore.ToString();
        totalScore.text = StatsController.Instance.totalScore.ToString();
        timeSpent.text = TimeUtil.SecondsToHMS(StatsController.Instance.timeSpent);
        numberOfGames.text = StatsController.Instance.numberOfGames.ToString();
    }

}
