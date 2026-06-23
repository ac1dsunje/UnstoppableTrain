using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private MainOverlayManager _mainOverlayPrefab;
    [SerializeField] private EventOverlayManager _eventOverlayPrefab;
    [SerializeField] private EndOverlayManager _endOverlayPrefab;

    private MainOverlayManager _mainOverlay;
    private EventOverlayManager _eventOverlay;
    private EndOverlayManager _endOverlay;
    private GameStateManager _gameStateManager;

    public UIManager Initialize(
        GameStateManager gameStateManager,
        TrainController train,
        GameEventsManager eventsManager,
        Canvas canvas)
    {
        _gameStateManager = gameStateManager;

        // Создаем оверлеи из префабов
        _mainOverlay = Instantiate(_mainOverlayPrefab, canvas.transform)
            .Initialize(train);

        _eventOverlay = Instantiate(_eventOverlayPrefab, canvas.transform)
            .Initialize(gameStateManager, eventsManager);

        _endOverlay = Instantiate(_endOverlayPrefab, canvas.transform);

        // Подписываемся на изменения состояний
        gameStateManager.OnStateChanged += OnStateChanged;

        // Показываем начальный экран
        _mainOverlay.ShowScreen();

        return this;
    }

    private void OnStateChanged(System.Type stateType)
    {
        // Скрываем все оверлеи
        _mainOverlay.HideScreen();
        _eventOverlay.HideScreen();
        _endOverlay.HideScreen();

        // Показываем нужный
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

    private void OnDestroy()
    {
        if (_gameStateManager != null)
        {
            _gameStateManager.OnStateChanged -= OnStateChanged;
        }
    }
}