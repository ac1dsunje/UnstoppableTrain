using System;
using UnityEngine;

public class TrainController: MonoBehaviour, Imovement
{
    [SerializeField] private TrainSO _data;
    [SerializeField] private GameObject _passengerPrefab;
    [SerializeField] private TrainStats _stats = new();

    private RoadController _currentRoad;
    public Action<TrainStats> OnStatsUpdated;

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

    public void TakeLayingMan(ManData _passenger)
    {
        if (_stats._passengers.Count >= _data.MaxAmount) return;

        _stats._passengers.Add(_passenger);
        OnStatsUpdated.Invoke(_stats);

        SpawnPassenger(_passenger, _stats.chunksPassed);
    }

    private void SpawnPassenger(ManData data, int currentChunkIndex)
    {
        Instantiate(_passengerPrefab, transform.position, Quaternion.identity, transform).GetComponent<PassengerController>().Initialize(this, data, currentChunkIndex);
    }

    public void GetPassengerOut(ManData data) 
    {
        _stats._passengers.Remove(data);
        OnStatsUpdated.Invoke(_stats);
    }
}