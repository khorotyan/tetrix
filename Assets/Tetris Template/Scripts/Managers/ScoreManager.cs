using UnityEngine;
using System.Collections;

public class ScoreManager : MonoBehaviour {

	public int currentScore=0;
	public int highScore;

    void Awake()
    {
        if (StatsController.Instance.highScore != 0)
        {
            highScore = StatsController.Instance.highScore;
            Managers.UI.inGameUI.UpdateScoreUI();
        }
        else
        {
            highScore = 0;
            Managers.UI.inGameUI.UpdateScoreUI();
        }
    }

	public void OnScore(int scoreIncreaseAmount)
	{	
		currentScore += scoreIncreaseAmount;
        CheckHighScore();
        Managers.UI.inGameUI.UpdateScoreUI();
        StatsController.Instance.totalScore += scoreIncreaseAmount;
    }

    public void CheckHighScore()
    {
        if (highScore < currentScore)
        {
            highScore = currentScore;
        }
    }

    public void ResetScore()
    {
        currentScore = 0;

        if (StatsController.Instance.highScore > highScore)
            highScore = StatsController.Instance.highScore;

        Managers.UI.inGameUI.UpdateScoreUI();
    }

}
