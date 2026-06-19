using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class LayingManController : MonoBehaviour
{

    public ManData Data;
    public Action<LayingManController> OnDeath;

    private void Awake()
    {
        LayDown();
    }

    private void Start()
    {
        RandomizeRoleAndTrait();
    }

    private void RandomizeRoleAndTrait()
    {
        Data.role = (Role)Random.Range(0, 3);
        Data.trait = (Trait)Random.Range(0, 3);
    }

    private void LayDown()
    {
        transform.Rotate(0, 0, 90f); // doesnt work as expected needs to be updated;
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
