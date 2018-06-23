using UnityEngine;
using System.Collections;

public class GameOverState : _StatesBase {

	#region implemented abstract members of _StatesBase
	public override void OnActivate ()
	{
        Managers.Game.isGameActive = false;
        Managers.Game.stats.highScore = Managers.Score.currentScore;
        Managers.Game.stats.numberOfGames++;
        Managers.UI.popUps.ActivateGameOverPopUp();
        Managers.Audio.PlayLoseSound();
       
        //Debug.Log ("<color=green>Game Over State</color> OnActive");	
	}

	public override void OnDeactivate ()
    {
        // Reset game score
        Managers.Score.ResetScore();

        // Count the number of user fails
        GameController.NumberOfFails++;

        if (GameController.NumberOfFails % 3 == 0)
            Managers.Adv.ShowRewardedAd();
	}

	public override void OnUpdate ()
	{
		//Debug.Log ("<color=yellow>Game Over State</color> OnUpdate");
	}
	#endregion

}
