using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoadController : MonoBehaviour
{
    private RoadSegmentConfigSO _config;
    private RailFactory _railFactory;

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

    public RoadController Initialize(
        RailFactory railFactory,
        RoadSegmentConfigSO segmentConfig)
    {
        _railFactory = railFactory;
        _config = segmentConfig;

        CreateRails();
        SubscribeToRailEvents();

        return this;
    }

    public void SetDependencies(TrainController train, GameStateManager gameStateManager)
    {
        _train = train;
        _gameStateManager = gameStateManager;
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

    private RailController CreateRail(float xOff, bool xFlip)
    {
        return _railFactory.Create(
            _config,
            new Vector3(transform.position.x + xOff, transform.position.y, transform.position.z),
            transform,
            xOff,
            xFlip
        );
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
                }
                remainingRail.ClearLayingMen();
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