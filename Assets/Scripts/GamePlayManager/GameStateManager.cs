using System;
using System.Collections.Generic;

public class GameStateManager
{
    private readonly Dictionary<Type, IGameState> _states = new();
    private readonly GameEventsManager _eventsManager;
    private IGameState _currentState;
    private bool _isEnd;

    public Action OnMoveLeft;
    public Action OnMoveRight;
    public Action<Type> OnStateChanged;

    public GameStateManager(GameEventsManager eventsManager)
    {
        _eventsManager = eventsManager;
    }

    public void RegisterState<T>(T state) where T : IGameState
    {
        _states[typeof(T)] = state;
    }

    public void EnterIn<T>() where T : IGameState
    {
        if (_isEnd) return;
        if (typeof(T) == typeof(EndState)) _isEnd = true;

        _currentState?.Exit();
        _currentState = _states[typeof(T)];
        _currentState.Enter();

        OnStateChanged?.Invoke(typeof(T));
    }

    public void TryEnterEventState()
    {
        if (_eventsManager.TryStartEvent())
        {
            EnterIn<EventState>();
        }
        else
        {
            EnterIn<MovingState>();
        }
    }

    public bool IsInState<T>() where T : IGameState
        => _currentState is T;

    public void TryMoveLeft()
    {
        if (!IsInState<ChoosingState>()) return;
        OnMoveLeft?.Invoke();
        EnterIn<MovingState>();
    }

    public void TryMoveRight()
    {
        if (!IsInState<ChoosingState>()) return;
        OnMoveRight?.Invoke();
        EnterIn<MovingState>();
    }
}