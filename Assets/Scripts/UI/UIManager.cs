using System;
using UnityEngine;
using Object = UnityEngine.Object;

public class UIManager: IDisposable
{
    private MainOverlayManager _mainOverlay;
    private EventOverlayManager _eventOverlay;
    private EndOverlayManager _endOverlay;
    private GameStateManager _gameStateManager;

    public UIManager(
        GameStateManager gameStateManager,
        TrainController train,
        GameEventsManager eventsManager,
        Canvas canvas,
        MainOverlayManager mainOverlayPrefab,
        EventOverlayManager eventOverlayPrefab,
        EndOverlayManager endOverlayPrefab,
        PassengerInfoSlotUIFactory passengerInfoSlotUIFactory)
    {
        _gameStateManager = gameStateManager;

        _mainOverlay = Object.Instantiate(mainOverlayPrefab, canvas.transform)
            .Initialize(train, passengerInfoSlotUIFactory);

        _eventOverlay = Object.Instantiate(eventOverlayPrefab, canvas.transform)
            .Initialize(gameStateManager, eventsManager);

        _endOverlay = Object.Instantiate(endOverlayPrefab, canvas.transform);

        gameStateManager.OnStateChanged += OnStateChanged;

        _mainOverlay.ShowScreen();
    }

    private void OnStateChanged(System.Type stateType)
    {
        _mainOverlay.HideScreen();
        _eventOverlay.HideScreen();
        _endOverlay.HideScreen();

        if (stateType == typeof(MovingState) || stateType == typeof(ChoosingState))
        {
            _mainOverlay.ShowScreen();
        }
        else if (stateType == typeof(EventState))
        {
            _eventOverlay.ShowScreen();
        }
        else if (stateType == typeof(EndState))
        {
            _endOverlay.ShowScreen();
        }
    }

    public void Dispose()
    {
        _gameStateManager.OnStateChanged -= OnStateChanged;
    }
}