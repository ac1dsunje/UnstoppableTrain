using System;
using UnityEngine;

public class LayingManController : MonoBehaviour
{
    [SerializeField] private int _minStationsNeeded = 1;
    [SerializeField] private int _maxStationsNeeded = 15;
    public ManData Data;
    public Action<LayingManController> OnDeath;
    public bool isActive { get; private set; }

    public void SetActiveState()
    {
        isActive = true;
    }

    private void Start()
    {
        Data = ManFactory.Create(
            minStations: _minStationsNeeded,
            maxStations: _maxStationsNeeded
        );
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Train"))
        {
            OnDeath.Invoke(this);
            Destroy(gameObject);
        }
    }
}
