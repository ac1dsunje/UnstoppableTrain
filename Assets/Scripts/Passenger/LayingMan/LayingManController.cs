using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class LayingManController : MonoBehaviour
{
    public ManData Data;
    public Action<LayingManController> OnDeath;

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

        Data.chunks = Random.Range(1, 7);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Train"))
        {
            OnDeath.Invoke(this);
            Destroy(gameObject);
        }
    }
}
