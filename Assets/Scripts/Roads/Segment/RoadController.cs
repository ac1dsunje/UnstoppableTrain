using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoadController : MonoBehaviour
{
    private RoadSegmentConfigSO _config;

    private float xOffset = 1.5f;

    public event Action<RoadController, bool> OnRoadStateChanged;

    public RoadType RoadType => _config.RoadType;
    public float RoadLength => _config.RoadLength;
    public bool IsLeftActive { get; private set; }
    public bool IsRightActive { get; private set; }

    private bool _isRoadActive;
    private RailController _leftRail;
    private RailController _rightRail;

    public RailController LeftRail => _leftRail;
    public RailController RightRail => _rightRail;

    private TrainController _train;
    private GameStateManager _gameStateManager;
    private ManGeneralConfigSO _manConfig;
    private ManVisualConfigSO _manVisualConfig;

    public RoadController Initialize(
        TrainController train,
        GameStateManager gameStateManager,
        ManGeneralConfigSO manConfig,
        ManVisualConfigSO manVisualConfig,
        RoadSegmentConfigSO segmentConfig)
    {
        _train = train;
        _gameStateManager = gameStateManager;
        _manConfig = manConfig;
        _manVisualConfig = manVisualConfig;
        _config = segmentConfig;

        CreateRails();
        InitializeRails();
        SubscribeToRailEvents();

        return this;
    }

    private void SubscribeToRailEvents()
    {
        _leftRail.OnThisActive += OnLeftRailStateChanged;
        _rightRail.OnThisActive += OnRightRailStateChanged;

        _leftRail.OnAllLayingMenDied += OnLeftRailCleared;
        _rightRail.OnAllLayingMenDied += OnRightRailCleared;
    }

    private void UnsubscribeFromRailEvents()
    {
        if (_leftRail != null)
        {
            _leftRail.OnThisActive -= OnLeftRailStateChanged;
            _leftRail.OnAllLayingMenDied -= OnLeftRailCleared;
        }

        if (_rightRail != null)
        {
            _rightRail.OnThisActive -= OnRightRailStateChanged;
            _rightRail.OnAllLayingMenDied -= OnRightRailCleared;
        }
    }

    private void OnDisable()
    {
        UnsubscribeFromRailEvents();
    }

    private void Start()
    {
        InitializeRoad();
    }

    private void CreateRails()
    {
        _leftRail = CreateRail(-xOffset, true);
        _rightRail = CreateRail(xOffset, false);
    }

    private void InitializeRails()
    {
        _leftRail.Initialize(_manConfig, _manVisualConfig);
        _rightRail.Initialize(_manConfig, _manVisualConfig);
    }

    private RailController CreateRail(float xOff, bool xFlip)
    {
        var rail = Instantiate(
            _config.RailPrefab,
            new Vector3(transform.position.x + xOff, transform.position.y, transform.position.z),
            Quaternion.identity,
            transform
        ).GetComponent<RailController>();

        int rand = Random.Range(0, _config.EnvironmentAtlas.EnvironmentObjects.Count);
        Transform railTransform = rail.transform;

        Transform envTransform = Instantiate(
            _config.EnvironmentAtlas.EnvironmentObjects[rand],
            new Vector3(railTransform.position.x + 2 * xOff, railTransform.position.y, railTransform.position.z),
            Quaternion.identity,
            railTransform
        ).transform;

        if (xFlip) envTransform.localScale = new Vector3(-1, 1, 1);

        return rail;
    }

    private void OnLeftRailStateChanged(bool state) { IsLeftActive = state; UpdateRoadState(); }
    private void OnRightRailStateChanged(bool state) { IsRightActive = state; UpdateRoadState(); }

    private void UpdateRoadState()
    {
        bool shouldBeActive = IsLeftActive || IsRightActive;

        if (_isRoadActive != shouldBeActive)
        {
            _isRoadActive = shouldBeActive;
            OnRoadStateChanged?.Invoke(this, _isRoadActive);

            if (_isRoadActive)
            {
                ActivateMenOnRails();
                OnRoadActivated();
            }
        }
    }

    private void ActivateMenOnRails()
    {
        foreach (var man in _leftRail.LayingMen) man.SetActiveState();
        foreach (var man in _rightRail.LayingMen) man.SetActiveState();
    }

    private void InitializeRoad()
    {
        switch (_config.RoadType)
        {
            case RoadType.Choosing:
                SpawnLayingMen();
                break;
            case RoadType.Moving:
            case RoadType.Station:
                break;
        }
    }

    private void OnRoadActivated()
    {
        switch (_config.RoadType)
        {
            case RoadType.Moving:
                if (_gameStateManager.TryEnterEventState())
                {
                    MediaEvents.TriggerEvent(transform.position, _config.OnEnterSound);
                }
                break;

            case RoadType.Choosing:
                _gameStateManager.EnterIn<ChoosingState>();
                MediaEvents.TriggerEvent(transform.position, _config.OnEnterSound);
                break;

            case RoadType.Station:
                if (_gameStateManager.TryEnterStationEvent())
                {
                    MediaEvents.TriggerEvent(transform.position, _config.OnEnterSound);
                }
                break;
        }
    }

    private void OnRailCleared(RailController clearedRail, RailController remainingRail)
    {
        switch (_config.RoadType)
        {
            case RoadType.Choosing:
                foreach (var passenger in remainingRail.LayingMen)
                {
                    _train.TryTakeNewPassenger(passenger.Data);
                    Destroy(passenger.gameObject);
                }
                break;

            case RoadType.Moving:
            case RoadType.Station:
                break;
        }
    }

    private void SpawnLayingMen()
    {
        int randLeft = Random.Range(1, _config.MaxMenOnTheRail + 1);
        LeftRail.SpawnManyLayingMen(randLeft);

        int randRight = Random.Range(1, _config.MaxMenOnTheRail + 1);
        RightRail.SpawnManyLayingMen(randRight);
    }

    private void OnLeftRailCleared() => OnRailCleared(_leftRail, _rightRail);
    private void OnRightRailCleared() => OnRailCleared(_rightRail, _leftRail);
}