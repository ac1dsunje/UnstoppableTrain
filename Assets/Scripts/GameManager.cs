using System;
using UnityEngine;
public enum GameState
{
    moving,
    checkingPassengers,
    choosing
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private InputHandler _input;

    [SerializeField] private GameState state;


    public Action OnMoveLeft;
    public Action OnMoveRight;

    private void Awake()
    {
        ChangeState(GameState.moving);
    }


    private void OnEnable()
    {
        _input.OnLeft += TryMoveLeft;
        _input.OnRight += TryMoveRight;
    }

    private void OnDisable()
    {
        _input.OnLeft -= TryMoveLeft;
        _input.OnRight -= TryMoveRight;
    }

    private void TryMoveLeft()
    {
        if (state != GameState.choosing) return;
            
        OnMoveLeft.Invoke();
        SetMovingState();
    }

    private void TryMoveRight()
    {
        if (state != GameState.choosing) return;

        OnMoveRight.Invoke();
        SetMovingState();
    }

    private void ChangeState(GameState newState)
    {
        state = newState;
    }

    public void SetChoosingState()
    {
        ChangeState(GameState.choosing);
        Time.timeScale = 0f;
    }

    public void SetMovingState()
    {
        ChangeState(GameState.moving);
        Time.timeScale = 1f;
    }
}