using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOverPopUp : MonoBehaviour {

    public Text gameOverScore;
    
    void OnEnable()
    {
        gameOverScore.text = Managers.Score.currentScore.ToString();
        GameController.isGameOverUiActive = true; // Gameover panel is enabled
        Managers.UI.panel.SetActive(true);
    }

    public void BackToMainMenu()
    {
        Managers.Grid.ClearBoard();
        Managers.Audio.PlayUIClick();
        Managers.UI.panel.SetActive(false);
        Managers.Game.SetState(typeof(MenuState));
        GameController.isGameOverUiActive = false; // Gameover panel is disabled
        gameObject.SetActive(false);
    }
}
