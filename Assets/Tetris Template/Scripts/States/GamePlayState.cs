using UnityEngine;
using System.Collections;

public class GamePlayState : _StatesBase {

    private float gamePlayDuration;

	#region implemented abstract members of _StatesBase
	public override void OnActivate ()
	{
        Managers.UI.panel.SetActive(false);
        Managers.UI.ActivateUI(Menus.INGAME);

        gamePlayDuration = Time.time;
        Managers.Cam.ZoomIn();

        if (GameController.Instance.isGamepadActive)
            GameController.Instance.controllsPanel.SetActive(true);
	}
	public override void OnDeactivate ()
	{
        StatsController.Instance.timeSpent += Time.time - gamePlayDuration;

        if (GameController.Instance.isGamepadActive)
            GameController.Instance.controllsPanel.SetActive(false);
    }

	public override void OnUpdate ()
	{
        if (Managers.Game.currentShape!=null)
            Managers.Game.currentShape.movementController.ShapeUpdate();
	}
	#endregion

}
