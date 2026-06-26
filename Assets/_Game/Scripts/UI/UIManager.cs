using System;

public class UIManager : IDisposable
{
    private readonly MainOverlayManager _mainOverlay;
    private readonly EventOverlayManager _eventOverlay;
    private readonly EndOverlayManager _endOverlay;
    private readonly GameStateManager _gameStateManager;

    public UIManager(
        GameStateManager gameStateManager,
        MainOverlayManager mainOverlay,
        EventOverlayManager eventOverlay,
        EndOverlayManager endOverlay)
    {
        _gameStateManager = gameStateManager;
        _mainOverlay = mainOverlay;
        _eventOverlay = eventOverlay;
        _endOverlay = endOverlay;

        _gameStateManager.OnStateChanged += OnStateChanged;

        _mainOverlay.ShowScreen();
    }

    private void OnStateChanged(Type stateType)
    {
        _mainOverlay.HideScreen();
        _eventOverlay.HideScreen();
        _endOverlay.HideScreen();

        if (stateType == typeof(MovingState) || stateType == typeof(ChoosingState))
            _mainOverlay.ShowScreen();
        else if (stateType == typeof(EventState))
            _eventOverlay.ShowScreen();
        else if (stateType == typeof(EndState))
            _endOverlay.ShowScreen();
    }

    public void Dispose()
    {
        _gameStateManager.OnStateChanged -= OnStateChanged;
    }
}