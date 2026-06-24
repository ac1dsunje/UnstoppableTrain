using UnityEngine;

public class PassengerFactory
{
    private readonly GameObject _passengerPrefab;
    private readonly ManDataFactory _manDataFactory;
    private readonly TraitFactory _traitFactory;

    public PassengerFactory(
        GameObject passengerPrefab,
        ManDataFactory manDataFactory,
        TraitFactory traitFactory)
    {
        _passengerPrefab = passengerPrefab;
        _manDataFactory = manDataFactory;
        _traitFactory = traitFactory;
    }

    public PassengerController Create(TrainController train, ManData data, Transform parent)
    {
        PassengerController passenger = Object.Instantiate(
            _passengerPrefab,
            train.transform.position,
            Quaternion.identity,
            parent
        ).GetComponent<PassengerController>();

        passenger.Initialize(train, data, _traitFactory);
        return passenger;
    }

    public PassengerController CreateWithRandomData(
        TrainController train,
        Transform parent,
        Role? role = null,
        Trait? trait = null,
        int? stationsNeeded = null)
    {
        ManData data = _manDataFactory.Create(null, role, trait, stationsNeeded);
        return Create(train, data, parent);
    }
}