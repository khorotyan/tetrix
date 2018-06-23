using UnityEngine;
using System.Collections;

public class RestartButton : MonoBehaviour {

    public void OnClickRestartButton()
    {
        Managers.Audio.PlayUIClick();
        Managers.Grid.ClearBoard();
        Managers.Score.ResetScore();
        Managers.Game.isGameActive = false;
        Managers.Game.SetState(typeof(GamePlayState));
        GameController.isGameOverUiActive = false; // Gameover panel is disabled
        Managers.UI.inGameUI.gameOverPopUp.SetActive(false);
    }
}
