using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrainController : MonoBehaviour, ITrainMovement
{
    [SerializeField] private TrainSO _data;
    [SerializeField] private GameObject _passengerPrefab;
    [SerializeField] private Transform _passengersContainer;

    private TrainStats _stats = new();
    public TrainStats GetStats => _stats;

    private RoadController _currentRoad;
    private float _speedScale = 1f;

    public event Action<TrainStats> OnStatsUpdated;
    public event Action OnStationPassed;
    public event Action OnAllDriversLeft;

    public TrainController Initialize()
    {
        SpawnInitialTeam();
        return this;
    }

    public void SetCurrentRoad(RoadController currentRoad)
    {
        _currentRoad = currentRoad;
        if (currentRoad.RoadType == RoadType.Station)
        {
            _stats.stationsPassed++;
            OnStationPassed?.Invoke();
            OnStatsUpdated?.Invoke(_stats);
        }
    }

    public RoadController GetCurrentRoad() => _currentRoad;
    public float GetSpeed() => _data.MoveSpeed * _speedScale;

    public void Stop()
    {
        _speedScale = 0f;
    }

    public void Resume()
    {
        _speedScale = 1f;
    }

    public List<PassengerController> GetPassengers() => _stats.Passengers;

    private int GetMaxCapacity() => _data.MaxAmount;

    public void TryTakeNewPassenger(ManData data)
    {
        if (_stats.Passengers.Count >= GetMaxCapacity()) return;
        SpawnPassenger(data);
    }

    public void GetPassengerOut(PassengerController passenger)
    {
        _stats.Passengers.Remove(passenger);

        if (passenger.GetData.role == Role.Driver &&
            !_stats.Passengers.Any(p => p.GetData.role == Role.Driver))
        {
            OnAllDriversLeft?.Invoke();
            return;
        }

        OnStatsUpdated?.Invoke(_stats);
    }

    private void SpawnInitialTeam()
    {
        SpawnPassenger(ManFactory.Create(role: Role.Driver, stationsNeeded: 3));
        SpawnPassenger(ManFactory.Create(role: Role.Mechanic, stationsNeeded: 2));
        SpawnPassenger(ManFactory.Create(role: Role.Doctor, stationsNeeded: 2));
    }

    private void SpawnPassenger(ManData data)
    {
        var passenger = Instantiate(_passengerPrefab, transform.position, Quaternion.identity, _passengersContainer)
            .GetComponent<PassengerController>()
            .Initialize(this, data);

        _stats.Passengers.Add(passenger);
        OnStatsUpdated?.Invoke(_stats);
    }
}