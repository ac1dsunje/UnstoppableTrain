using UnityEngine;

public class PassengerInfoSlotUIFactory : PooledComponentFactory<PassengerInfoSlotUI>
{
    private GameObject _passengerInfoSlotUIPrefab;

    public PassengerInfoSlotUIFactory(
        GameObject passengerInfoSlotUIPrefab,
        PoolConfig poolConfig) : base(poolConfig)
    {
        _passengerInfoSlotUIPrefab = passengerInfoSlotUIPrefab;
    }

    public PassengerInfoSlotUI Get(Transform parent, ManData data)
    {
        var slot = GetItem(_passengerInfoSlotUIPrefab);
        slot.transform.SetParent(parent, false);
        slot.Initialize(data);
        return slot;
    }

    protected override PassengerInfoSlotUI Create(GameObject prefab)
    {
        var slot = Object.Instantiate(prefab).GetComponent<PassengerInfoSlotUI>();
        return slot;
    }
}