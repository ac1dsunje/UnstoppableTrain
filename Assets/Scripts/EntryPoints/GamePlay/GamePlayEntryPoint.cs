using System;
using UnityEngine;

public class GamePlayEntryPoint : MonoBehaviour
{
    [Header("Game")]
    [SerializeField] private InputHandler _input;
    [SerializeField] private CameraController _cam;
    [SerializeField] private CoroutineRunner _coroutineRunner;
    [SerializeField] private RoadsConfigSO _roadConfig;
    [SerializeField] private float _messageDelay = 1.2f;

    [Header("Train")]
    [SerializeField] private GameObject _trainPrefab;
    [SerializeField] private Vector3 TrainSpawnPosition = new(1.5f, 1.3f, -20f);
    [SerializeField] private TrainSO _trainData;

    [Header("UI")]
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private Canvas _canvas;

    [Header("Difficulty")]
    [SerializeField] private DifficultySO difficulty;

    [Header("People")]
    [SerializeField] private ManGeneralConfigSO _manConfig;
    [SerializeField] private ManVisualConfigSO _manVisualConfig;

    [Header("Passenger")]
    [SerializeField] private GameObject _passengerPrefab;

    private GameStateManager _gameStateManager;
    private TrainController _train;
    private GameEventsManager _eventsManager;
    private RoadManager _roadManager;

    private void Awake()
    {
        NameSelector nameSelector = new();
        RoleSelector roleSelector = new(difficulty.roleLevel.Weights);
        TraitSelector traitSelector = new(difficulty.traitLevel.Weights);
        StationsSelector stationsSelector = new(difficulty.stationsLevel.Range);

        ManDataFactory manDataFactory = new(
            nameSelector,
            roleSelector,
            traitSelector,
            stationsSelector
        );

        TraitFactory traitFactory = new(difficulty.traitsConfig);

        PassengerFactory passengerFactory = new(
            _passengerPrefab,
            manDataFactory,
            traitFactory
        );

        LayingManFactory layingManFactory = new(_manConfig, _manVisualConfig, manDataFactory);
        RailFactory railFactory = new(layingManFactory);
        RoadFactory roadFactory = new(railFactory);

        _train = SpawnTrain();
        _train.Initialize(passengerFactory, _trainData);
        _cam.Initialize(_train.transform);

        _eventsManager = new(_coroutineRunner, _train, _messageDelay);

        _gameStateManager = BuildGameStateManager(_train);

        _train.GetComponent<TrainMovement>().Initialize(_gameStateManager);

        var roadsParent = new GameObject("Roads").transform;
        _roadManager = new(_roadConfig, _coroutineRunner, roadsParent, roadFactory);
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
    }

    private void OnDisable()
    {
        _input.OnLeft -= _gameStateManager.TryMoveLeft;
        _input.OnRight -= _gameStateManager.TryMoveRight;
        _input.OnRestart -= TryRestart;
    }

    private void TryRestart()
    {
        if (!_gameStateManager.IsInState<EndState>()) return;

        _roadManager.Dispose();
        _gameStateManager.Dispose();
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
}