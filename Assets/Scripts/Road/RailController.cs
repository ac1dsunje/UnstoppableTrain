using System;
using System.Collections.Generic;
using UnityEngine;

public class RailController : MonoBehaviour
{
    [SerializeField] private GameObject _passengerPrefab;
    private List<LayingManController> _passengers = new();
    public List<LayingManController> Passengers => _passengers;


    public Action<bool> OnThisActive;
    public Action OnAllPassengersDied;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Train")
        {
            SetRailActive();
        }
    }

    public void SpawnPassengers(int count)
    {
        for (int i = 0; i < count; i++) 
        {
            SpawnPassenger(i);
        }
    }

    private void SpawnPassenger(int step)
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z + step * 2);
        LayingManController _passenger = Instantiate(_passengerPrefab, pos, Quaternion.identity, transform).GetComponent<LayingManController>();
        _passengers.Add(_passenger);
        _passenger.OnDeath += OnPassengerDeath;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Train")
        {
            SetRailUnActive();
        }
    }

    private void SetRailActive()
    {
        OnThisActive.Invoke(true);
    }

    private void SetRailUnActive()
    {
        OnThisActive.Invoke(false);
    }

    private void OnDestroy()
    {
        foreach(var passenger in _passengers)
        {
            passenger.OnDeath -= OnPassengerDeath;
        }
    }

    private void OnPassengerDeath(LayingManController passenger)
    {
        _passengers.Remove(passenger);

        if (_passengers.Count == 0)
        {
            OnAllPassengersDied.Invoke();
        }
    }
}
