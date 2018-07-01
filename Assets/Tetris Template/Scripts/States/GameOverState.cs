using UnityEngine;
using System.Collections;

public class GameOverState : _StatesBase {

	#region implemented abstract members of _StatesBase
	public override void OnActivate ()
	{
        Managers.Game.isGameActive = false;
        StatsController.Instance.highScore = Managers.Score.currentScore;
        StatsController.Instance.numberOfGames++;
        Managers.UI.popUps.ActivateGameOverPopUp();
        Managers.Audio.PlayLoseSound();
        // Update the PlayGames highscore
        PlayGamesController.AddScoreToLeaderboard(GPGSIds.leaderboard_highscore, StatsController.Instance.highScore);
    }

	public override void OnDeactivate ()
    {
        // Reset game score
        Managers.Score.ResetScore();

        // Count the number of user fails
        GameController.NumberOfFails++;

        if (GameController.NumberOfFails % 5 == 0)
            Managers.Adv.ShowDefaultAd();
	}

	public override void OnUpdate ()
	{
		//Debug.Log ("<color=yellow>Game Over State</color> OnUpdate");
	}
	#endregion

}
