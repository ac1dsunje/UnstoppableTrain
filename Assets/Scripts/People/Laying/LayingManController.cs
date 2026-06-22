using System;
using UnityEngine;

public class LayingManController : MonoBehaviour
{
    public ManData Data { get; private set; }
    public Action<LayingManController> OnDeath;
    public bool isActive { get; private set; }

    public void SetActiveState()
    {
        isActive = true;
    }

    private void Start()
    {
        Data = ManFactory.Create();
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
