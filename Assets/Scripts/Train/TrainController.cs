using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class TrainController: MonoBehaviour, Imovement
{
    [SerializeField] private TrainSO _data;
    [SerializeField] private GameObject _passengerPrefab;
    [SerializeField] private Transform _passengersContainer;
    private TrainStats _stats = new();

    private RoadController _currentRoad;
    public Action<TrainStats> OnStatsUpdated;

    private void Awake()
    {
        SpawnFirstPassenger();
    }

    public void SetCurrentRoad(RoadController currentRoad)
    {
        _currentRoad = currentRoad;

        _stats.chunksPassed++;
        OnStatsUpdated.Invoke(_stats);
    }

    public RoadController GetCurrentRoad()
    {
        return _currentRoad;
    }

    public float GetSpeed()
    {
        return _data.MoveSpeed;
    }

    private int GetMaxCapacity()
    {
        return _data.MaxAmount;
    }

    private void SpawnFirstPassenger()
    {
        ManData _data = new();

        _data.role = Role.Driver;

        int count = Enum.GetValues(typeof(Trait)).Length;
        _data.trait = (Trait)Random.Range(0, count);

        _data.StationsNeeded = 10;

        SpawnPassenger(_data);
    }

    public void TakeLayingMan(ManData _data)
    {
        if (_stats._passengers.Count >= GetMaxCapacity()) return;

        SpawnPassenger(_data);
    }

    private void SpawnPassenger(ManData data)
    {
        _stats._passengers.Add(data);
        OnStatsUpdated.Invoke(_stats);
        Instantiate(_passengerPrefab, transform.position, Quaternion.identity, _passengersContainer).GetComponent<PassengerController>().Initialize(this, data);
    }

    public void GetPassengerOut(ManData data) 
    {
        _stats._passengers.Remove(data);
        OnStatsUpdated.Invoke(_stats);
    }
}