using UnityEngine;

public class PassengerFactory : PooledComponentFactory<PassengerController>
{
    private readonly GameObject _passengerPrefab;
    private readonly ManDataFactory _manDataFactory;
    private readonly TraitBehaviourFactory _traitFactory;

    public PassengerFactory(
        GameObject passengerPrefab,
        ManDataFactory manDataFactory,
        TraitBehaviourFactory traitFactory,
        PoolConfig poolConfig) : base(poolConfig)
    {
        _passengerPrefab = passengerPrefab;
        _manDataFactory = manDataFactory;
        _traitFactory = traitFactory;
    }

    public PassengerController Get(TrainController train, ManData data, Transform parent)
    {
        var item = GetItem(_passengerPrefab);

        item.transform.SetParent(parent.transform, false);

        item.Initialize(train, data, _traitFactory);

        return item;
    }

    public PassengerController GetWithRandomData(TrainController train,
        Transform parent,
        Role? role = null,
        Trait? trait = null,
        int? stationsNeeded = null)
    {
        ManData data = _manDataFactory.Create(null, role, trait, stationsNeeded);

        return Get(train, data, parent);
    }

    protected override PassengerController Create(GameObject prefab)
    {
        PassengerController passenger = Object.Instantiate(
            prefab
        ).GetComponent<PassengerController>();

        return passenger;
    }
}