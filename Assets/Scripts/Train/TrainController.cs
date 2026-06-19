using System.Collections.Generic;
using UnityEngine;

public class TrainController: MonoBehaviour, Imovement
{
    [SerializeField] private TrainSO _data;

    [SerializeField] private List<ManData> _passengers = new();
    private RoadController _currentRoad;

    public void SetCurrentRoad(RoadController currentRoad)
    {
        _currentRoad = currentRoad;
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
        _passengers.Add(_passenger);

        //ToDo: spawn Passengers / (not list?)
    }
}