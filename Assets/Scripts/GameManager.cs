using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public enum GameState
{
    moving,
    social,
    disaster,
    choosing,
    station
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private InputHandler _input;
    [SerializeField] private CameraController _cam;
    [SerializeField] private TrainController _train;
    [SerializeField] private SocialEventManager _socialManager;

    private GameState state;

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
        _train.SetSpeedScale(0f);
        _cam.SetChoosingPos();

        OnStateChanged?.Invoke(state);
    }

    public void SetMovingState()
    {
        ChangeState(GameState.moving);
        _train.SetSpeedScale(1f);
        _cam.SetMovingPos();

        OnStateChanged?.Invoke(state);
    }

    public void SetStationState()
    {
        ChangeState(GameState.station);
        _train.SetSpeedScale(0f);
        _cam.SetChoosingPos();

        StartCoroutine(WaitAtStation());
        OnStateChanged?.Invoke(state);
    }

    private IEnumerator WaitAtStation()
    {
        Debug.Log("passengers getting out.. please wait");
        yield return new WaitForSeconds(2f);
        SetMovingState();
    }

    public void SetEventState()
    {
        if (Random.value < 0.5f)
        {
            SetSocialState();
        }
        else
        {
            SetDisasterState();
        }
    }

    private void SetSocialState()
    {
        if (!_socialManager.TryStartSocialPhase())
        {
            return;
        }

        ChangeState(GameState.social);
        _train.SetSpeedScale(0f);
        _cam.SetChoosingPos();

        OnStateChanged?.Invoke(state);
    }

    private void SetDisasterState()
    {
        if (!_socialManager.TryStartSocialPhase())
        {
            return;
        }

        ChangeState(GameState.social);
        _train.SetSpeedScale(0f);
        _cam.SetChoosingPos();

        OnStateChanged?.Invoke(state);
    }
}