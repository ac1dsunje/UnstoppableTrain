using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    moving,
    choosing,
    station,
    @event,
    end
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private InputHandler _input;
    [SerializeField] private CameraController _cam;
    [SerializeField] private TrainController _train;
    [SerializeField] private GameEventsManager _eventsManager;

    [SerializeField] private GameState state;

    public Action OnMoveLeft;
    public Action OnMoveRight;
    public Action<GameState> OnStateChanged;

    private void Awake()
    {
        ChangeState(GameState.moving);
    }

    private void OnEnable()
    {
        _input.OnLeft += TryMoveLeft;
        _input.OnRight += TryMoveRight;
        _input.OnRestart += TryRestartGame;
        _train.OnAllDriversLeft += SetEndState;
    }

    private void OnDisable()
    {
        _input.OnLeft -= TryMoveLeft;
        _input.OnRight -= TryMoveRight;
        _input.OnRestart -= TryRestartGame;
        _train.OnAllDriversLeft -= SetEndState;
    }

    private void TryMoveLeft()
    {
        if (state != GameState.choosing) return;
        OnMoveLeft?.Invoke();
        SetMovingState();
    }

    private void TryMoveRight()
    {
        if (state != GameState.choosing) return;
        OnMoveRight?.Invoke();
        SetMovingState();
    }

    private void TryRestartGame()
    {
        if (state != GameState.end) return;
        SceneManager.LoadScene("GamePlay");
    }

    private void ChangeState(GameState newState)
    {
        if (state == GameState.end) return;
        state = newState;
        OnStateChanged?.Invoke(state);
    }

    public void SetMovingState()
    {
        _train.SetSpeedScale(1f);
        _cam.SetMovingPos();
        ChangeState(GameState.moving);
    }

    public void SetChoosingState()
    {
        _train.SetSpeedScale(0f);
        _cam.SetChoosingPos();
        ChangeState(GameState.choosing);
    }

    public void SetStationState()
    {
        _train.SetSpeedScale(0f);
        _cam.SetChoosingPos();
        ChangeState(GameState.station);
        StartCoroutine(WaitAtStation());
    }

    private IEnumerator WaitAtStation()
    {
        // ToDo : add passengers getting out animation
        Debug.Log("Passengers getting out... please wait");
        yield return new WaitForSeconds(2f);
        SetMovingState();
    }

    public void SetEventState()
    {
        bool started = _eventsManager.TryStartEvent();
        if (!started) return;

        _train.SetSpeedScale(0f);
        _cam.SetChoosingPos();
        ChangeState(GameState.@event);
    }

    private void SetEndState()
    {
        _train.SetSpeedScale(0f);
        ChangeState(GameState.end);
    }
}