using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TrainController : MonoBehaviour, Imovement
{
    [SerializeField] private TrainSO _data;
    [SerializeField] private GameObject _passengerPrefab;
    [SerializeField] private Transform _passengersContainer;
    [SerializeField] private TrainStats _stats = new();

    private RoadController _currentRoad;

    // Общие изменения статистики (список пассажиров, capacity и т.п.) — для UI.
    public Action<TrainStats> OnStatsUpdated;

    // Факт проезда станции — для пассажиров (декремент StationsLeft).
    public Action OnStationPassed;

    private float _speedScale = 1f;

    private void Start()
    {
        SpawnFirstPassenger();
    }

    public void SetCurrentRoad(RoadController currentRoad)
    {
        _currentRoad = currentRoad;

        _stats.chunksPassed++;

        // 1. Сначала уведомляем пассажиров — они декрементируют StationsLeft,
        //    и те, кому нужно, выйдут (внутри Leave дёрнется OnStatsUpdated).
        OnStationPassed?.Invoke();

        // 2. Затем обновляем UI: список пассажиров уже актуален,
        //    а chunksPassed — свежий.
        OnStatsUpdated?.Invoke(_stats);
    }

    public RoadController GetCurrentRoad() => _currentRoad;

    public float GetSpeed() => _data.MoveSpeed * _speedScale;

    public void SetSpeedScale(float speed) => _speedScale = speed;

    private int GetMaxCapacity() => _data.MaxAmount;

    private void SpawnFirstPassenger()
    {
        ManData data = new ManData
        {
            role = Role.Driver,
            trait = (Trait)Random.Range(0, Enum.GetValues(typeof(Trait)).Length),
            StationsNeeded = 10
        };

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