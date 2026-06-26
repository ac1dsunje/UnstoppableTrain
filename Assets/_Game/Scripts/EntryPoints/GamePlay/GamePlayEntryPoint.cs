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
    [SerializeField] private TrainDataSO _trainData;

    [Header("UI")]
    [SerializeField] private MainOverlayManager _mainOverlayPrefab;
    [SerializeField] private EventOverlayManager _eventOverlayPrefab;
    [SerializeField] private EndOverlayManager _endOverlayPrefab;

    [SerializeField] private Canvas _canvas;

    [Header("Sound")]
    [SerializeField] private GameObject _sfxPrefab;

    [Header("Difficulty")]
    [SerializeField] private DifficultySO difficulty;

    [Header("People")]
    [SerializeField] private ManGeneralConfigSO _manConfig;
    [SerializeField] private GameObject _layingManPrefab;

    [Header("Passenger")]
    [SerializeField] private GameObject _passengerPrefab;
    [SerializeField] private GameObject _passengerInfoSlotUIPrefab;

    [Header("Pools")]
    [SerializeField] private PoolConfig _roadPoolConfig;
    [SerializeField] private PoolConfig _railsPoolConfig;
    [SerializeField] private PoolConfig _environmentPoolConfig;
    [SerializeField] private PoolConfig _layingManPoolConfig;
    [SerializeField] private PoolConfig _passengerPoolConfig;
    [SerializeField] private PoolConfig _soundPoolConfig;

    private GameStateManager _gameStateManager;
    private TrainController _train;
    private ITrainMovementStrategy _movementStrategy;
    private GameEventsManager _eventsManager;
    private RoadManager _roadManager;
    private SFXManager _soundFXManager;
    private UIManager _uiManager;

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

        TraitBehaviourFactory traitBehaviourFactory = new(difficulty.traitsConfig);

        PassengerFactory passengerFactory = new(
            _passengerPrefab,
            manDataFactory,
            traitBehaviourFactory,
            _passengerPoolConfig
        );

        MediaEventsBus mediaEventsBus = new();

        SkinHatFactory hatFactory = new(_layingManPoolConfig);

        SkinManagerFactory skinManagerFactory = new(hatFactory);

        LayingManFactory layingManFactory = new(_manConfig, _layingManPrefab, manDataFactory, _layingManPoolConfig, skinManagerFactory, mediaEventsBus);
        EnvironmentFactory environmentFactory = new(_environmentPoolConfig);
        RailFactory railFactory = new(layingManFactory, _railsPoolConfig);
        RoadFactory roadFactory = new(railFactory, environmentFactory, _roadConfig.RoadPrefab, _roadPoolConfig, mediaEventsBus);

        var sfxParent = new GameObject("Sounds").transform;
        SoundFactory soundFactory = new(_sfxPrefab, _soundPoolConfig, sfxParent);
        _soundFXManager = new SFXManager(soundFactory, _coroutineRunner, mediaEventsBus);

        _train = SpawnTrain();

        _movementStrategy = new StandardMovementStrategy(switchRailsSpeed: 5f);

        _train.Initialize(passengerFactory, _trainData, _movementStrategy);

        _cam.Initialize(_train.transform);

        _eventsManager = new(_coroutineRunner, _train, _messageDelay);

        _gameStateManager = BuildGameStateManager(_train);

        _movementStrategy.BindInput(_gameStateManager);

        RoadSelector roadSelector = new(_roadConfig.SegmentConfigs);

        var roadsParent = new GameObject("Roads").transform;
        _roadManager = new(_roadConfig, roadsParent, roadFactory, roadSelector, _gameStateManager, _train);

        PassengerInfoSlotUIFactory passengerInfoSlotUIFactory = new(_passengerInfoSlotUIPrefab, _passengerPoolConfig);

        var mainOverlay = Object.Instantiate(_mainOverlayPrefab, _canvas.transform)
            .Initialize(_train, passengerInfoSlotUIFactory);

        var eventOverlay = Object.Instantiate(_eventOverlayPrefab, _canvas.transform)
            .Initialize(_gameStateManager, _eventsManager);

        var endOverlay = Object.Instantiate(_endOverlayPrefab, _canvas.transform);

        _uiManager = new UIManager(_gameStateManager, mainOverlay, eventOverlay, endOverlay);
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

        _movementStrategy?.UnbindInput();
    }

    private void TryRestart()
    {
        if (!_gameStateManager.IsInState<EndState>()) return;

        _roadManager.Dispose();
        _soundFXManager.Dispose();
        _gameStateManager.Dispose();
        _uiManager.Dispose();
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