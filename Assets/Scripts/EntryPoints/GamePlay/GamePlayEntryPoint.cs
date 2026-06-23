using System;
using UnityEngine;

public class GamePlayEntryPoint : MonoBehaviour
{
    [Header("Game")]
    [SerializeField] private InputHandler _input;
    [SerializeField] private CameraController _cam;
    [SerializeField] private CoroutineRunner _coroutineRunner;
    [SerializeField] private RoadConfigSO _roadConfig;
    [SerializeField] private float _messageDelay = 1.2f;

    [Header("Train")]
    [SerializeField] private GameObject _trainPrefab;
    [SerializeField] private Vector3 TrainSpawnPosition = new(1.5f, 1.3f, -20f);

    [Header("UI")]
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private Canvas _canvas;

    [Header("Preset")]
    [SerializeField] private DifficultySO difficulty;

    private GameStateManager _gameStateManager;
    private TrainController _train;
    private GameEventsManager _eventsManager;
    private RoadManager _roadManager;
    private Action _onAllDriversLeft;

    private void Awake()
    {
        InitializeFactories();

        _train = SpawnTrain();
        _train.Initialize();
        _cam.Initialize(_train.transform);

        _eventsManager = new GameEventsManager(_coroutineRunner, _train, _messageDelay);

        _gameStateManager = BuildGameStateManager(_train);
        _onAllDriversLeft = () => _gameStateManager.EnterIn<EndState>();

        _train.GetComponent<TrainMovement>().Initialize(_gameStateManager);

        var roadsParent = new GameObject("Roads").transform;
        _roadManager = new RoadManager(_roadConfig, _coroutineRunner, roadsParent);
        _roadManager.Initialize(_gameStateManager, _train);

        _uiManager.Initialize(_gameStateManager, _train, _eventsManager, _canvas);
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

        _roadManager.Dispose();
        StartCoroutine(SceneLoader.RestartGameAsync());
    }

    private TrainController SpawnTrain()
    {
        return Instantiate(_trainPrefab, TrainSpawnPosition, Quaternion.identity)
            .GetComponent<TrainController>();
    }

    private GameStateManager BuildGameStateManager(TrainController train)
    {
        var gsm = new GameStateManager(_eventsManager, _train);

        gsm.RegisterState(new MovingState(train, _cam));
        gsm.RegisterState(new ChoosingState(train, _cam));
        gsm.RegisterState(new EventState(train, _cam));
        gsm.RegisterState(new EndState(train));

        return gsm;
    }

    private void InitializeFactories()
    {
        RoleSelector.SetWeights(difficulty.roleLevel.Weights);
        TraitSelector.SetWeights(difficulty.traitLevel.Weights);
        StationsSelector.SetRange(difficulty.stationsLevel.Range);
    }
}