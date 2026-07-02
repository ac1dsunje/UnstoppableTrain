using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrainController
{
    private readonly Transform _passengersContainer;
    private readonly ITrainView _view;
    private readonly TrainModel _model;
    private readonly PassengerFactory _passengerFactory;
    private readonly AudioSource _trainMovingSound;

    public event Action<TrainStats> OnStatsUpdated;
    public event Action OnStationPassed;
    public event Action OnAllDriversLeft;
    public TrainController(PassengerFactory passengerFactory, TrainModel model, ITrainView view, AudioSource trainMovingSound, Transform passengersContainer)
    {
        _passengerFactory = passengerFactory;
        _model = model;
        _view = view;
        _trainMovingSound = trainMovingSound;
        _passengersContainer = passengersContainer;
        SpawnInitialTeam();
    }

    public void SetCurrentRoad(RoadController currentRoad)
    {
        _model.CurrentRoad = currentRoad;

        if (currentRoad.Config.IsStation)
        {
            _model.Stats.stationsPassed++;
            OnStationPassed?.Invoke();
            OnStatsUpdated?.Invoke(_model.Stats);
        }
    }

    public void Stop() 
    {
        _view.SetSpeed(0f * _model.MoveSpeed);
        _trainMovingSound.Stop();
    }
    public void Resume()
    {
        _view.SetSpeed(1f * _model.MoveSpeed);
        _trainMovingSound.Play();
    }

    public void MoveLeft()
    {
        var currentRoad = _model.CurrentRoad;
        if (currentRoad == null || currentRoad.IsLeftActive) return;
        _view.Move(currentRoad.LeftRail.transform);
    }

    public void MoveRight()
    {
        var currentRoad = _model.CurrentRoad;
        if (currentRoad == null || currentRoad.IsRightActive) return;
        _view.Move(currentRoad.RightRail.transform);
    }

    public List<PassengerController> GetPassengers() => _model.Stats.Passengers;
    public TrainStats GetStats() => _model.Stats;

    public void TryTakeNewPassenger(ManData data)
    {
        if (_model.Stats.Passengers.Count >= _model.MaxAmount) return;

        PassengerController passenger = _passengerFactory.Get(this, data, _passengersContainer);
        AddPassengerToStats(passenger);
    }

    public void GetPassengerOut(PassengerController passenger)
    {
        _model.Stats.Passengers.Remove(passenger);
        _passengerFactory.Release(passenger);

        if (passenger.GetData.role == Role.Driver &&
            !_model.Stats.Passengers.Any(p => p.GetData.role == Role.Driver))
        {
            OnAllDriversLeft?.Invoke();
            return;
        }

        OnStatsUpdated?.Invoke(_model.Stats);
    }

    private void SpawnInitialTeam()
    {
        PassengerController driver = _passengerFactory.GetWithRandomData(this, _passengersContainer, role: Role.Driver, stationsNeeded: 3);
        AddPassengerToStats(driver);

        PassengerController mechanic = _passengerFactory.GetWithRandomData(this, _passengersContainer, role: Role.Mechanic, stationsNeeded: 2);
        AddPassengerToStats(mechanic);

        PassengerController doctor = _passengerFactory.GetWithRandomData(this, _passengersContainer, role: Role.Doctor, stationsNeeded: 2);
        AddPassengerToStats(doctor);
    }

    private void AddPassengerToStats(PassengerController passenger)
    {
        _model.Stats.Passengers.Add(passenger);
        OnStatsUpdated?.Invoke(_model.Stats);
    }
}