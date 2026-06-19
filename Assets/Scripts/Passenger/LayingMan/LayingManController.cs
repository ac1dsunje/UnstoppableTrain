using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class LayingManController : MonoBehaviour
{
    public ManData Data;
    public Action<LayingManController> OnDeath;

    private void Start()
    {
        RandomizeRoleAndTrait();
    }

    private void RandomizeRoleAndTrait()
    {
        Data.role = (Role)Random.Range(0, 4);
        Data.trait = (Trait)Random.Range(0, 4);
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
