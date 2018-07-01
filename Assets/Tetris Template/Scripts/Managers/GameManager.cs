using UnityEngine;

public class GameManager : MonoBehaviour
{
	public bool isGameActive;
    public TetrisShape currentShape;
    public Transform blockHolder;

    void Awake()
	{
		isGameActive = false;
	}

	private _StatesBase currentState;
	public _StatesBase State
	{
		get { return currentState; }
	}

	//Changes the current game state
	public void SetState(System.Type newStateType)
	{
		if (currentState != null)
		{
			currentState.OnDeactivate();
		}

		currentState = GetComponentInChildren(newStateType) as _StatesBase;
		if (currentState != null)
		{
			currentState.OnActivate();
		}
	}

	void Update()
	{
		if (currentState != null)
		{
			currentState.OnUpdate();
		}
	}

	void Start()
	{
		SetState(typeof(MenuState));
	}


}