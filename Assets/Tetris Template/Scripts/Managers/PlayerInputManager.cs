using UnityEngine;
using System.Collections;

public enum InputMethod
{
    KeyboardInput,
    MouseInput,
    TouchInput
}

public class PlayerInputManager : MonoBehaviour
{
    public InputMethod inputType;

    void Awake()
    {
        if (Application.platform == RuntimePlatform.Android)
            inputType = InputMethod.TouchInput;
        else
            inputType = InputMethod.KeyboardInput;
    }

    private void Update()
    {
        if (GameController.isGamePaused == false)
        {
            if (inputType == InputMethod.TouchInput)
                TouchInput();
            else
                KeyboardInput();
        }
    }

    #region KEYBOARD
    void KeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.A))
            Managers.Game.currentShape.movementController.RotateClockWise(false);
        else if (Input.GetKeyDown(KeyCode.D))
            Managers.Game.currentShape.movementController.RotateClockWise(true);

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            Managers.Game.currentShape.movementController.MoveHorizontal(Vector2.left);
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            Managers.Game.currentShape.movementController.MoveHorizontal(Vector2.right);
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (Managers.Game.currentShape != null)
            {
                Managers.Game.currentShape.movementController.InstantFall();
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (Managers.Game.currentShape != null)
            {
                Managers.Game.currentShape.movementController.StopInstantFall();
            }
        }
    }
    #endregion

    #region MOUSE
    Vector2 _startPressPosition;
    Vector2 _endPressPosition;
    Vector2 _currentSwipe;
    float _buttonDownPhaseStart;
    public float tapInterval;

    void MouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //save began touch 2d point
            _startPressPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            _buttonDownPhaseStart = Time.time;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (Time.time - _buttonDownPhaseStart > tapInterval)
            {
                //save ended touch 2d point
                _endPressPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

                //create vector from the two points
                _currentSwipe = new Vector2(_endPressPosition.x - _startPressPosition.x, _endPressPosition.y - _startPressPosition.y);

                //normalize the 2d vector
                _currentSwipe.Normalize();

                //swipe left
                if (_currentSwipe.x < 0 && _currentSwipe.y > -0.5f && _currentSwipe.y < 0.5f)
                {
                    Managers.Game.currentShape.movementController.MoveHorizontal(Vector2.left);
                }
                //swipe right
                if (_currentSwipe.x > 0 && _currentSwipe.y > -0.5f && _currentSwipe.y < 0.5f)
                {
                    Managers.Game.currentShape.movementController.MoveHorizontal(Vector2.right);
                }

                //swipe down
                if (_currentSwipe.y < 0 && _currentSwipe.x > -0.5f && _currentSwipe.x < 0.5f)
                {
                    if (Managers.Game.currentShape != null)
                    {
                        Managers.Game.currentShape.movementController.InstantFall();
                    }
                }
            }
            else
            {
                if (_startPressPosition.x < Screen.width / 2)
                    Managers.Game.currentShape.movementController.RotateClockWise(false);
                else
                    Managers.Game.currentShape.movementController.RotateClockWise(true);
            }
        }
    }
    #endregion

    #region TOUCH
    void TouchInput()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                _startPressPosition = touch.position;
                _endPressPosition = touch.position;
                _buttonDownPhaseStart = Time.time;
            }
            if (touch.phase == TouchPhase.Moved)
            {
                _endPressPosition = touch.position;

            }
            if (touch.phase == TouchPhase.Ended)
            {
                if (GameController.Instance.isGamepadActive == false)
                {
                    if (Time.time - _buttonDownPhaseStart > tapInterval)
                    {
                        //save ended touch 2d point
                        _endPressPosition = new Vector2(touch.position.x, touch.position.y);

                        //create vector from the two points
                        _currentSwipe = new Vector2(_endPressPosition.x - _startPressPosition.x, _endPressPosition.y - _startPressPosition.y);

                        //normalize the 2d vector
                        _currentSwipe.Normalize();

                        //swipe left
                        if (_currentSwipe.x < 0 && _currentSwipe.y > -0.85f && _currentSwipe.y < 0.85f)
                        {
                            Managers.Game.currentShape.movementController.MoveHorizontal(Vector2.left);
                        }
                        //swipe right
                        if (_currentSwipe.x > 0 && _currentSwipe.y > -0.85f && _currentSwipe.y < 0.85f)
                        {
                            Managers.Game.currentShape.movementController.MoveHorizontal(Vector2.right);
                        }
                        //swipe down
                        if (_currentSwipe.y < 0 && _currentSwipe.x > -0.85f && _currentSwipe.x < 0.85f)
                        {
                            if (Managers.Game.currentShape != null)
                            {
                                Managers.Game.currentShape.movementController.InstantFall();
                            }
                        }
                        //swipe up
                        if (_currentSwipe.y > 0 && _currentSwipe.x > -0.85f && _currentSwipe.x < 0.85f)
                        {
                            if (Managers.Game.currentShape != null)
                            {
                                Managers.Game.currentShape.movementController.StopInstantFall();
                            }
                        }
                    }
                }
                else /*if (_currentSwipe.x + _currentSwipe.y< 0.5f */
                {
                    if (_startPressPosition.x < Screen.width / 2)
                        Managers.Game.currentShape.movementController.RotateClockWise(false);
                    else
                        Managers.Game.currentShape.movementController.RotateClockWise(true);
                }
            }
        }

    }
    #endregion

}
