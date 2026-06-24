using System;
using UnityEngine;
using Random = UnityEngine.Random;

public enum RoadType { Moving, Choosing, Station }

public abstract class RoadController : MonoBehaviour
{
    [SerializeField] protected RoadSegmentConfigSO _config;

    private float xOffset = 1.5f;

    public event Action<RoadController, bool> OnRoadStateChanged;

    public abstract RoadType GetRoadType { get; }

    public float RoadLength => _config.roadLength;
    public bool IsLeftActive { get; private set; }
    public bool IsRightActive { get; private set; }

    private bool _isRoadActive;
    private RailController _leftRail;
    private RailController _rightRail;

    public RailController LeftRail => _leftRail;
    public RailController RightRail => _rightRail;

    protected TrainController _train;
    protected GameStateManager _gameStateManager;
    protected ManGeneralConfigSO _manConfig;
    protected ManVisualConfigSO _manVisualConfig;

    public RoadController Initialize(
        TrainController train,
        GameStateManager gameStateManager,
        ManGeneralConfigSO manConfig,
        ManVisualConfigSO manVisualConfig)
    {
        _train = train;
        _gameStateManager = gameStateManager;
        _manConfig = manConfig;
        _manVisualConfig = manVisualConfig;

        InitializeRails();

        return this;
    }

    private void Awake()
    {
        CreateRails();
    }

    private void OnEnable()
    {
        _leftRail.OnThisActive += OnLeftRailStateChanged;
        _rightRail.OnThisActive += OnRightRailStateChanged;

        _leftRail.OnAllLayingMenDied += OnLeftRailCleared;
        _rightRail.OnAllLayingMenDied += OnRightRailCleared;
    }

    private void OnDisable()
    {
        _leftRail.OnThisActive -= OnLeftRailStateChanged;
        _rightRail.OnThisActive -= OnRightRailStateChanged;

        _leftRail.OnAllLayingMenDied -= OnLeftRailCleared;
        _rightRail.OnAllLayingMenDied -= OnRightRailCleared;
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

        int rand = Random.Range(0, _config._environmentAtlas.EnvironmentObjects.Count);
        Transform railTransform = rail.transform;

        Transform envTransform = Instantiate(
            _config._environmentAtlas.EnvironmentObjects[rand],
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

    protected virtual void InitializeRoad() { }
    protected virtual void OnRoadActivated() { }
    protected virtual void OnRailCleared(RailController clearedRail, RailController remainingRail) { }

    private void OnLeftRailCleared() => OnRailCleared(_leftRail, _rightRail);
    private void OnRightRailCleared() => OnRailCleared(_rightRail, _leftRail);
}