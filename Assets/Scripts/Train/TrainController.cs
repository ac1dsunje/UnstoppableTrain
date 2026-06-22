using System;
using System.Collections.Generic;
using UnityEngine;

public class TrainController : MonoBehaviour, Imovement
{
    [SerializeField] private TrainSO _data;
    [SerializeField] private GameObject _passengerPrefab;
    [SerializeField] private Transform _passengersContainer;

    private TrainStats _stats = new();

    private RoadController _currentRoad;

    public Action<TrainStats> OnStatsUpdated;

    public Action OnStationPassed;

    private float _speedScale = 1f;

    private void Start()
    {
        SpawnFirstPassenger();
    }

    public void SetCurrentRoad(RoadController currentRoad)
    {
        _currentRoad = currentRoad;
        if (currentRoad.GetRoadType == RoadType.Station)
        {
            _stats.stationsPassed++;
            OnStationPassed.Invoke();
            OnStatsUpdated.Invoke(_stats);
        }
    }

    public RoadController GetCurrentRoad() => _currentRoad;

    public float GetSpeed() => _data.MoveSpeed * _speedScale;

    public void SetSpeedScale(float speed) => _speedScale = speed;

    private int GetMaxCapacity() => _data.MaxAmount;

    public List<PassengerController> GetPassengers() => _stats.Passengers;

    private void SpawnFirstPassenger()
    {
        ManData data = ManFactory.Create(
            role: Role.Driver,
            stationsNeeded: 10
        );

        SpawnPassenger(data);
    }

    public void TakeLayingMan(ManData data)
    {
        if (_stats.Passengers.Count >= GetMaxCapacity()) return;
        SpawnPassenger(data);
    }

    private void SpawnPassenger(ManData data)
    {
        var passenger = Instantiate(_passengerPrefab, transform.position, Quaternion.identity, _passengersContainer)
            .GetComponent<PassengerController>()
            .Initialize(this, data);

        _stats.Passengers.Add(passenger);
        OnStatsUpdated?.Invoke(_stats);
    }

    public void GetPassengerOut(PassengerController passenger)
    {
        _stats.Passengers.Remove(passenger);
        OnStatsUpdated?.Invoke(_stats);
    }
}