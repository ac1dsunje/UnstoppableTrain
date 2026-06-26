using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoadController : MonoBehaviour
{
    private RoadSegmentConfigSO _config;
    private RailFactory _railFactory;
    private EnvironmentFactory _environmentFactory;
    private RoadContext _context;

    private Transform _railContainer;
    private Transform _environmentContainer;

    public event Action<RoadController, bool> OnRoadStateChanged;

    public RoadSegmentConfigSO Config => _config;
    public float RoadLength => _config.RoadLength;
    public bool IsLeftActive { get; private set; }
    public bool IsRightActive { get; private set; }

    private bool _isRoadActive;

    public RailController LeftRail { get; private set; }
    public RailController RightRail { get; private set; }
    private GameObject _leftEnvironment;
    private GameObject _rightEnvironment;

    public RoadController Initialize(
        RailFactory railFactory,
        EnvironmentFactory environmentFactory,
        RoadSegmentConfigSO segmentConfig,
        TrainController train,
        GameStateManager gameStateManager)
    {
        _railFactory = railFactory;
        _environmentFactory = environmentFactory;
        _config = segmentConfig;
        _context = new RoadContext(this, train, gameStateManager);

        return this;
    }

    public void SetContainers()
    {
        _railContainer = new GameObject("RailsContainer").transform;
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

        _config.OnSetup(_context);
    }

    private void CreateRails()
    {
        float offset = _config.RailXOffset;
        LeftRail = CreateRail(-offset);
        RightRail = CreateRail(offset);
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
        float envOffset = _config.RailXOffset * _config.EnvironmentXMultiplier;
        _leftEnvironment = CreateEnvironment(-envOffset, true);
        _rightEnvironment = CreateEnvironment(envOffset, false);
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
            new Vector3(transform.position.x + xOff, transform.position.y, transform.position.z),
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
        if (LeftRail != null) _railFactory.Release(LeftRail);
        if (RightRail != null) _railFactory.Release(RightRail);

        if (_leftEnvironment != null) _environmentFactory.Release(_leftEnvironment);
        if (_rightEnvironment != null) _environmentFactory.Release(_rightEnvironment);
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

    private void OnRoadActivated()
    {
        _config.OnActivated(_context);
    }

    private void OnRailCleared(RailController clearedRail, RailController remainingRail)
    {
        _config.OnRailCleared(_context, clearedRail, remainingRail);
    }

    private void OnLeftRailCleared() => OnRailCleared(LeftRail, RightRail);
    private void OnRightRailCleared() => OnRailCleared(RightRail, LeftRail);
}