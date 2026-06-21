using System;
using UnityEngine;
using Random = UnityEngine.Random;

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
        SetRandomData();
    }

    private void SetRandomData()
    {
        int count = Enum.GetValues(typeof(Role)).Length;
        Data.role = (Role)Random.Range(0, count);

        count = Enum.GetValues(typeof(Trait)).Length;
        Data.trait = (Trait)Random.Range(0, count);

        Data.StationsNeeded = Random.Range(_minStationsNeeded, _maxStationsNeeded+1);
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
