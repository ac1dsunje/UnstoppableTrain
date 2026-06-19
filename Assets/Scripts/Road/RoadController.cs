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
    public RoadType GetRoadType => _roadType;
    public float RoadLength => roadLength;

    private bool _isLeftActive = false;
    private bool _isRightActive = false;
    private bool _isRoadActive = false;

    private RailController _leftRail;
    private RailController _rightRail;

    private TrainController _train;

    private void OnEnable()
    {
        _leftRail.OnThisActive += OnLeftRailStateChanged;
        _rightRail.OnThisActive += OnRightRailStateChanged;

        _leftRail.OnAllPassengersDied += TakeAllRight;
        _rightRail.OnAllPassengersDied += TakeAllLeft;
    }

    private void OnDisable()
    {
        _leftRail.OnThisActive -= OnLeftRailStateChanged;
        _rightRail.OnThisActive -= OnRightRailStateChanged;

        _leftRail.OnAllPassengersDied -= TakeAllRight;
        _rightRail.OnAllPassengersDied -= TakeAllLeft;
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
        _rightRail = Instantiate(RailPrefab, new Vector3(transform.position.x + 1.5f, transform.position.y, transform.position.z), Quaternion.identity, transform).GetComponent<RailController>();
    }

    private void TrySpawnPassengers()
    {
        if (_roadType == RoadType.Choosing)
        {

            int randLeft = Random.Range(1, 3);
            _leftRail.SpawnPassengers(randLeft);

            int randRight = Random.Range(1, 3);
            _rightRail.SpawnPassengers(randRight);
        }
    }


    private void OnLeftRailStateChanged(bool state)
    {
        _isLeftActive = state;
        UpdateRoadState();
    }

    private void OnRightRailStateChanged(bool state)
    {
        _isRightActive = state;
        UpdateRoadState();
    }

    private void UpdateRoadState()
    {
        bool shouldBeActive = _isLeftActive || _isRightActive;

        if (_isRoadActive != shouldBeActive)
        {
            _isRoadActive = shouldBeActive;
            OnRoadStateChanged?.Invoke(this, _isRoadActive);
        }
    }

    private void TakeAllLeft()
    {
        foreach (var passenger in _leftRail.Passengers)
        {
            _train.TakePassenger(passenger.Data);
            Destroy(passenger);
        }
    }

    private void TakeAllRight()
    {
        foreach (var passenger in _rightRail.Passengers)
        {
            _train.TakePassenger(passenger.Data);
            Destroy(passenger);
        }
    }
}