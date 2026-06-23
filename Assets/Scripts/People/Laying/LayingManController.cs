using System;
using UnityEngine;

public class LayingManController : MonoBehaviour, ISkin
{
    [SerializeField] private SoundData _onDeathSound;
    [SerializeField] private MeshRenderer _shape;

    public ManData Data { get; private set; }
    public event Action<LayingManController> OnDeath;
    public event Action<ManData> OnManDataInitialized;

    public bool isActive { get; private set; }

    public MeshRenderer GetShape() => _shape;

    public void SetActiveState()
    {
        isActive = true;
    }

    private void Start()
    {
        Data = ManFactory.Create();

        OnManDataInitialized.Invoke(Data);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Train"))
        {
            OnDeath.Invoke(this);
            MediaEvents.TriggerEvent(transform.position, _onDeathSound);
            Destroy(gameObject);
        }
    }
}
