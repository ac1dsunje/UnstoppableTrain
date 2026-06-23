using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private MainOverlayManager _mainOverlayManager;
    private EventOverlayManager _eventOverlayManager;
    private EndOverlayManager _endOverlayManager;

    public UIManager Initialize(
        GameStateManager gameStateManager,
        TrainController train,
        GameEventsManager eventsManager,
        MainOverlayManager mainOverlay,
        EventOverlayManager eventOverlay,
        EndOverlayManager endOverlay)
    {
        _mainOverlayManager = mainOverlay.Initialize(train);
        _eventOverlayManager = eventOverlay.Initialize(gameStateManager, eventsManager);
        _endOverlayManager = endOverlay;

        gameStateManager.OnStateChanged += HandleStateChanged;

        return this;
    }

    private void HandleStateChanged(Type stateType)
    {
        if (stateType == typeof(MovingState) ||
            stateType == typeof(ChoosingState))
        {
            _mainOverlayManager.ShowScreen();
            _eventOverlayManager.HideScreen();
            _endOverlayManager.HideScreen();
        }
        else if (stateType == typeof(EventState))
        {
            _mainOverlayManager.HideScreen();
            _eventOverlayManager.ShowScreen();
            _endOverlayManager.HideScreen();
        }
        else if (stateType == typeof(EndState))
        {
            _mainOverlayManager.HideScreen();
            _eventOverlayManager.HideScreen();
            _endOverlayManager.ShowScreen();
        }
    }
}