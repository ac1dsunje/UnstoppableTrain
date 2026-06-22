using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private TrainController _train;
    [SerializeField] private GameEventsManager _eventsManager;

    private MainOverlayManager _mainOverlayManager;
    private EventOverlayManager _eventOverlayManager;

    private void OnEnable() => _gameManager.OnStateChanged += HandleStateChanged;
    private void OnDisable() => _gameManager.OnStateChanged -= HandleStateChanged;

    private void Awake()
    {
        _mainOverlayManager = GetComponent<MainOverlayManager>().Initialize(_train);
        _eventOverlayManager = GetComponent<EventOverlayManager>().Initialize(_gameManager, _eventsManager);
    }

    private void HandleStateChanged(GameState state)
    {
        switch (state)
        {
            case GameState.moving:
            case GameState.choosing:
            case GameState.station:
                _mainOverlayManager.ShowScreen();
                _eventOverlayManager.HideScreen();
                break;

            case GameState.@event:
                _mainOverlayManager.HideScreen();
                _eventOverlayManager.ShowScreen();
                break;
        }
    }
}