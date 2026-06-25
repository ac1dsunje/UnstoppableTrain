using System;
using UnityEngine;

public class LayingManController : MonoBehaviour
{
    [SerializeField] private MeshRenderer _shape;

    private ManGeneralConfigSO _config;
    private ManDataFactory _manDataFactory;
    private ISkinComponent _skinManager;

    public ManData Data { get; private set; }
    public event Action<LayingManController> OnDeath;

    public bool IsActive { get; private set; }

    public void Initialize(ManGeneralConfigSO config, ManDataFactory manDataFactory)
    {
        _config = config;
        _manDataFactory = manDataFactory;
        _skinManager = SkinManagerFactory.Create(_config.SkinConfig, _shape);
    }

    public void SetActiveState()
    {
        IsActive = true;
    }

    public void SetupData()
    {
        Data = _manDataFactory.Create();
        _skinManager?.Apply(Data);
        IsActive = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Train"))
        {
            OnDeath?.Invoke(this);
            MediaEvents.TriggerEvent(transform.position, _config.ManConfig.OnDeathSound);
        }
    }
}