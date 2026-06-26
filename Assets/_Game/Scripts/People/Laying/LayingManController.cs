using System;
using UnityEngine;

public class LayingManController : MonoBehaviour
{
    [SerializeField] private MeshRenderer _shape;
    [SerializeField] private Transform HatHolder; 

    private ManGeneralConfigSO _config;
    private ManDataFactory _manDataFactory;
    private ISkinComponent _skinManager;
    private SkinManagerFactory _skinManagerFactory;
    private MediaEventsBus _mediaEventsBus;

    public ManData Data { get; private set; }
    public event Action<LayingManController> OnDeath;

    public bool IsActive { get; private set; }

    public void Initialize(ManGeneralConfigSO config, ManDataFactory manDataFactory, SkinManagerFactory skinManagerFactory, MediaEventsBus mediaEventsBus)
    {
        _config = config;
        _manDataFactory = manDataFactory;
        _skinManagerFactory = skinManagerFactory;
        _mediaEventsBus = mediaEventsBus;

        _skinManager = _skinManagerFactory.Create(_config.SkinConfig, _shape, HatHolder);
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
            _mediaEventsBus.TriggerEvent(transform.position, _config.SoundConfig.OnDeathSound);
        }
    }
}