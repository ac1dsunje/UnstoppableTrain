using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePlayEntryPoint : MonoBehaviour
{
    [Header("Game")]
    [SerializeField] private InputHandler _input;
    [SerializeField] private CameraController _cam;
    [SerializeField] private GameEventsManager _eventsManager;
    [SerializeField] private CoroutineRunner _coroutineRunner;
    [SerializeField] private RoadManager _roadManager;

    [Header("Train")]
    [SerializeField] private GameObject _trainPrefab;
    [SerializeField] private Vector3 TrainSpawnPosition = new(1.5f, 1.3f, -20f);

    [Header("UI")]
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private MainOverlayManager _mainOverlay;
    [SerializeField] private EventOverlayManager _eventOverlay;
    [SerializeField] private EndOverlayManager _endOverlay;

    [Header("Presets")]
    [SerializeField] private RolePreset rolePreset;
    [SerializeField] private TraitPreset traitPreset;
    [SerializeField] private StationsPreset stationtPreset;

    private GameStateManager _gameStateManager;
    private TrainController _train;
    private Action _onAllDriversLeft;

    private void Awake()
    {
        InitializeFactories();

        _train = SpawnTrain();
        _train.Initialize();
        _cam.Initialize(_train.transform);
        _eventsManager.Initialize(_train);

        _gameStateManager = BuildGameStateManager(_train);
        _onAllDriversLeft = () => _gameStateManager.EnterIn<EndState>();

        _train.GetComponent<TrainMovement>().Initialize(_gameStateManager);
        _roadManager.Initialize(_gameStateManager, _train);
        _uiManager.Initialize(_gameStateManager, _train, _eventsManager,
                              _mainOverlay, _eventOverlay, _endOverlay);
    }

    private void Start()
    {
        _gameStateManager.EnterIn<MovingState>();
    }

    private void OnEnable()
    {
        _input.OnLeft += _gameStateManager.TryMoveLeft;
        _input.OnRight += _gameStateManager.TryMoveRight;
        _input.OnRestart += TryRestart;
        _train.OnAllDriversLeft += _onAllDriversLeft;
    }

    private void OnDisable()
    {
        _input.OnLeft -= _gameStateManager.TryMoveLeft;
        _input.OnRight -= _gameStateManager.TryMoveRight;
        _input.OnRestart -= TryRestart;
        _train.OnAllDriversLeft -= _onAllDriversLeft;
    }

    private void TryRestart()
    {
        if (!_gameStateManager.IsInState<EndState>()) return;

        StartCoroutine(SceneLoader.RestartGameAsync());
    }

    private TrainController SpawnTrain()
    {
        return Instantiate(_trainPrefab, TrainSpawnPosition, Quaternion.identity)
            .GetComponent<TrainController>();
    }

    private GameStateManager BuildGameStateManager(TrainController train)
    {
        var gsm = new GameStateManager(_eventsManager);

        gsm.RegisterState(new MovingState(train, _cam));
        gsm.RegisterState(new ChoosingState(train, _cam));
        gsm.RegisterState(new StationState(train, _cam, gsm, _coroutineRunner));
        gsm.RegisterState(new EventState(train, _cam));
        gsm.RegisterState(new EndState(train));

        return gsm;
    }

    private void InitializeFactories()
    {
        RoleSelector.SetWeights(rolePreset.Weights);
        TraitSelector.SetWeights(traitPreset.Weights);
        StationsSelector.SetRange(stationtPreset.Range);
    }
}