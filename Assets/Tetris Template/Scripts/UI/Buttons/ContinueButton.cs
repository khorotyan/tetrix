using UnityEngine;
using System.Collections;

public class ContinueButton : MonoBehaviour {

    public void OnClickContinueButton()
    {
        Managers.Audio.PlayUIClick();
        Managers.Game.SetState(typeof(GamePlayState));
        GameController.CloseAdWatchPanel();
        GameController.Instance.OnGameContinueClick();
    }
}
