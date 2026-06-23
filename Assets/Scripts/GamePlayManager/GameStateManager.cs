using System;
using System.Collections.Generic;
using System.Linq;

public class GameStateManager: IDisposable
{
    private readonly Dictionary<Type, IGameState> _states = new();
    private readonly GameEventsManager _eventsManager;
    private readonly TrainController _train;
    private IGameState _currentState;
    private bool _isEnd;

    public event Action OnMoveLeft;
    public event Action OnMoveRight;
    public event Action<Type> OnStateChanged;

    public GameStateManager(GameEventsManager eventsManager, TrainController train)
    {
        _eventsManager = eventsManager;
        _train = train;

        _train.OnAllDriversLeft += OnAllDriversLeft;
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

    public void TryEnterStationEvent()
    {
        List<PassengerController> passengers = new(_train.GetPassengers());

        bool shouldEnterStation = passengers.Any(p => p.CheckLeave());

        if (shouldEnterStation)
        {
            if (_eventsManager.TryEnterStationEvent(passengers))
            {
                EnterIn<EventState>();
            }
        }
    }

    private void OnAllDriversLeft()
    {
        EnterIn<EndState>();
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

    public void Dispose()
    {
        _train.OnAllDriversLeft -= OnAllDriversLeft;
        _states.Clear();
    }
}