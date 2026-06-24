using System;
using UnityEngine;

public class LayingManController : MonoBehaviour
{
    [SerializeField] private MeshRenderer _shape;
    [SerializeField] private LayingManUI _ui;

    private ManGeneralConfigSO _config;
    private ISkinApplier _skinManager;

    public ManData Data { get; private set; }
    public event Action<LayingManController> OnDeath;

    public bool IsActive { get; private set; }

    public void Initialize(ManGeneralConfigSO config)
    {
        _config = config;
        _skinManager = SkinManagerFactory.Create(_config.SkinConfig, _shape);
        _ui.Initialize(this);
    }

    public void SetActiveState()
    {
        IsActive = true;
    }

    private void Start()
    {
        Data = ManFactory.Create();
        _skinManager.ApplySkin(Data);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Train"))
        {
            OnDeath?.Invoke(this);
            MediaEvents.TriggerEvent(transform.position, _config.ManConfig.OnDeathSound);
            Destroy(gameObject);
        }
    }
}