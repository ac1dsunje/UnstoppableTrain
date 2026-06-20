using System;
using UnityEngine;

public class TrainController: MonoBehaviour, Imovement
{
    [SerializeField] private TrainSO _data;

    private RoadController _currentRoad;
    [SerializeField] private TrainStats _stats = new();
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

    public void TakePassenger(ManData _passenger)
    {
        _stats._passengers.Add(_passenger);
        OnStatsUpdated.Invoke(_stats);

        //ToDo: spawn Passengers / (not list?)
    }
}