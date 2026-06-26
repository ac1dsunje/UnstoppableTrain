using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoadController : MonoBehaviour
{
    private RoadSegmentConfigSO _config;
    private RailFactory _railFactory;
    private EnvironmentFactory _environmentFactory;
    private readonly float xOffset = 1.5f;

    private Transform _railContainer;
    private Transform _environmentContainer;

    public event Action<RoadController, bool> OnRoadStateChanged;
    public RoadType RoadType => _config.RoadType;
    public float RoadLength => _config.RoadLength;
    public bool IsLeftActive { get; private set; }
    public bool IsRightActive { get; private set; }

    private bool _isRoadActive;

    public RailController LeftRail { get; private set; }
    public RailController RightRail { get; private set; }
    private GameObject _leftEnvironment;
    private GameObject _rightEnvironment;

    private TrainController _train;
    private GameStateManager _gameStateManager;

    public RoadController Initialize(
        RailFactory railFactory,
        EnvironmentFactory environmentFactory,
        RoadSegmentConfigSO segmentConfig)
    {
        _railFactory = railFactory;
        _environmentFactory = environmentFactory;
        _config = segmentConfig;

        return this;
    }

    public void SetContainers()
    {
        _railContainer = new GameObject("RailsContainter").transform;
        _railContainer.SetParent(transform);
        _environmentContainer = new GameObject("EnvironmentsContainer").transform;
        _environmentContainer.SetParent(transform);
    }

    public void SetupData()
    {
        IsLeftActive = false;
        IsRightActive = false;
        _isRoadActive = false;

        CreateRails();
        CreateEnvironments();

        SubscribeToRailEvents();
        InitializeRoad();
    }

    public void SetDependencies(TrainController train, GameStateManager gameStateManager)
    {
        _train = train;
        _gameStateManager = gameStateManager;
    }

    private void CreateRails()
    {
        LeftRail = CreateRail(-xOffset);
        RightRail = CreateRail(xOffset);
    }

    private RailController CreateRail(float xOff)
    {
        return _railFactory.Get(
            _config,
            new Vector3(transform.position.x + xOff, transform.position.y, transform.position.z),
            _railContainer
        );
    }

    private void CreateEnvironments()
    {
        _leftEnvironment = CreateEnvironment(-xOffset, true);
        _rightEnvironment = CreateEnvironment(xOffset, false);
    }

    private GameObject CreateEnvironment(float xOff, bool xFlip)
    {
        if (_config.EnvironmentAtlas == null || _config.EnvironmentAtlas.EnvironmentObjects.Count == 0)
            return null;

        int rand = Random.Range(0, _config.EnvironmentAtlas.EnvironmentObjects.Count);
        var prefab = _config.EnvironmentAtlas.EnvironmentObjects[rand];

        Vector3 scale = xFlip ? new Vector3(-1, 1, 1) : Vector3.one;

        return _environmentFactory.Get(
            prefab,
            new Vector3(transform.position.x + 3 * xOff, transform.position.y, transform.position.z),
            _environmentContainer,
            scale
        );
    }

    private void SubscribeToRailEvents()
    {
        LeftRail.OnThisActive += OnLeftRailStateChanged;
        LeftRail.OnAllLayingMenDied += OnLeftRailCleared;

        RightRail.OnThisActive += OnRightRailStateChanged;
        RightRail.OnAllLayingMenDied += OnRightRailCleared;
    }

    private void UnsubscribeFromRailEvents()
    {
        LeftRail.OnThisActive -= OnLeftRailStateChanged;
        LeftRail.OnAllLayingMenDied -= OnLeftRailCleared;

        RightRail.OnThisActive -= OnRightRailStateChanged;
        RightRail.OnAllLayingMenDied -= OnRightRailCleared;
    }

    private void OnDisable()
    {
        UnsubscribeFromRailEvents();
        ReleaseAll();
    }

    private void ReleaseAll()
    {
        _railFactory.Release(LeftRail);
        _railFactory.Release(RightRail);

        _environmentFactory.Release(_leftEnvironment);
        _environmentFactory.Release(_rightEnvironment);
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
        foreach (var man in LeftRail.LayingMen) man.SetActiveState();
        foreach (var man in RightRail.LayingMen) man.SetActiveState();
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

    private void OnLeftRailCleared() => OnRailCleared(LeftRail, RightRail);
    private void OnRightRailCleared() => OnRailCleared(RightRail, LeftRail);
}