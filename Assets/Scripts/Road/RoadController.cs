using System;
using UnityEngine;
using Random = UnityEngine.Random;

public enum RoadType
{
    Moving,
    Choosing
}

public class RoadController : MonoBehaviour
{
    public Action<RoadController, bool> OnRoadStateChanged;

    [SerializeField] private float roadLength = 10f;

    [SerializeField] private GameObject RailPrefab;
    [SerializeField] private RoadType _roadType;
    [SerializeField] private int _maxMenOnTheRail = 3;
    public RoadType GetRoadType => _roadType;
    public float RoadLength => roadLength;

    public bool IsLeftActive { get; private set; }
    public bool IsRightActive { get; private set; }
    private bool _isRoadActive;

    private RailController _leftRail;
    private RailController _rightRail;

    public RailController LeftRail => _leftRail;
    public RailController RightRail => _rightRail;

    private TrainController _train;

    private void OnEnable()
    {
        _leftRail.OnThisActive += OnLeftRailStateChanged;
        _rightRail.OnThisActive += OnRightRailStateChanged;

        _leftRail.OnAllLayingMenDied += TakeAllRight;
        _rightRail.OnAllLayingMenDied += TakeAllLeft;
    }

    private void OnDisable()
    {
        _leftRail.OnThisActive -= OnLeftRailStateChanged;
        _rightRail.OnThisActive -= OnRightRailStateChanged;

        _leftRail.OnAllLayingMenDied -= TakeAllRight;
        _rightRail.OnAllLayingMenDied -= TakeAllLeft;
    }

    private void Awake()
    {
        CreateRails();
    }

    public void SetTrainLink(TrainController controller)
    {
        _train = controller;
    }

    private void Start()
    {
        TrySpawnPassengers();
    }

    private void CreateRails()
    {
        _leftRail = Instantiate(RailPrefab, new Vector3(transform.position.x - 1.5f, transform.position.y, transform.position.z), Quaternion.identity, transform).GetComponent<RailController>();
        // ToDo : add enviroment
        _rightRail = Instantiate(RailPrefab, new Vector3(transform.position.x + 1.5f, transform.position.y, transform.position.z), Quaternion.identity, transform).GetComponent<RailController>();
        // ToDo : add enviroment
    }

    private void TrySpawnPassengers()
    {
        if (_roadType == RoadType.Choosing)
        {

            int randLeft = Random.Range(1, _maxMenOnTheRail+1);
            _leftRail.SpawnManyLayingMen(randLeft);

            int randRight = Random.Range(1, _maxMenOnTheRail + 1);
            _rightRail.SpawnManyLayingMen(randRight);
        }
    }

    private void OnLeftRailStateChanged(bool state)
    {
        IsLeftActive = state;
        UpdateRoadState();
    }

    private void OnRightRailStateChanged(bool state)
    {
        IsRightActive = state;
        UpdateRoadState();
    }

    private void UpdateRoadState()
    {
        bool shouldBeActive = IsLeftActive || IsRightActive;

        if (_isRoadActive != shouldBeActive)
        {
            _isRoadActive = shouldBeActive;
            OnRoadStateChanged?.Invoke(this, _isRoadActive);
        }
    }

    private void TakeAllLeft()
    {
        foreach (var passenger in _leftRail.LayingMen)
        {
            _train.TakeLayingMan(passenger.Data);
            Destroy(passenger.gameObject);
        }
    }

    private void TakeAllRight()
    {
        foreach (var passenger in _rightRail.LayingMen)
        {
            _train.TakeLayingMan(passenger.Data);
            Destroy(passenger.gameObject);
        }
    }
}