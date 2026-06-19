using System.Collections.Generic;
using UnityEngine;

public class TrainController: MonoBehaviour, Imovement
{
    [SerializeField] private TrainSO _data;

    [SerializeField] private List<ManData> _passengers = new();

    public float GetSpeed()
    {
        return _data.MoveSpeed;
    }

    public void TakePassenger(ManData _passenger)
    {
        Debug.Log($"Took the man with role {_passenger.role} & trait {_passenger.trait}");
        _passengers.Add(_passenger);

        //ToDo: spawn Passengers / (not list?)
    }
}