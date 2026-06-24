using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrainController : MonoBehaviour, ITrainMovement
{
    [SerializeField] private Transform _passengersContainer;

    private TrainSO _data;
    private TrainStats _stats = new();
    public TrainStats GetStats => _stats;

    private RoadController _currentRoad;
    private float _speedScale = 1f;
    private PassengerFactory _passengerFactory;

    public event Action<TrainStats> OnStatsUpdated;
    public event Action OnStationPassed;
    public event Action OnAllDriversLeft;

    public TrainController Initialize(PassengerFactory passengerFactory, TrainSO data)
    {
        _data = data;
        _passengerFactory = passengerFactory;
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

        PassengerController passenger = _passengerFactory.Create(this, data, _passengersContainer);
        AddPassengerToStats(passenger);
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
        PassengerController driver = _passengerFactory.CreateWithRandomData(this, _passengersContainer, role: Role.Driver, stationsNeeded: 3);
        AddPassengerToStats(driver);

        PassengerController mechanic = _passengerFactory.CreateWithRandomData(this, _passengersContainer, role: Role.Mechanic, stationsNeeded: 2);
        AddPassengerToStats(mechanic);

        PassengerController doctor = _passengerFactory.CreateWithRandomData(this, _passengersContainer, role: Role.Doctor, stationsNeeded: 2);
        AddPassengerToStats(doctor);
    }

    private void AddPassengerToStats(PassengerController passenger)
    {
        _stats.Passengers.Add(passenger);
        OnStatsUpdated?.Invoke(_stats);
    }
}