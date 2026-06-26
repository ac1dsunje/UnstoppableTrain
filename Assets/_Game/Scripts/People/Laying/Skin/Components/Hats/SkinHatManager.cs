using UnityEngine;

public class SkinHatManager : ISkinComponent
{
    private readonly Transform _hatHolder;
    private readonly SkinHatSO _config;
    private readonly SkinHatFactory _hatFactory;

    private GameObject _currentHat;

    public SkinHatManager(Transform hatHolder, SkinHatSO config, SkinHatFactory hatFactory)
    {
        _hatHolder = hatHolder;
        _config = config;
        _hatFactory = hatFactory;
    }

    public void Apply(ManData data)
    {
        ApplyHat(data.role);
    }

    private void ApplyHat(Role role)
    {
        if (_currentHat != null)
        {
            _hatFactory.Release(_currentHat);
            _currentHat = null;
        }

        var prefab = GetHatPrefab(role);

        if (prefab != null)
        {
            _currentHat = _hatFactory.Get(prefab, _hatHolder);
        }
    }

    private GameObject GetHatPrefab(Role role)
    {
        return role switch
        {
            Role.Doctor => _config.DoctorHatPrefab,
            Role.Driver => _config.DriverHatPrefab,
            Role.Mechanic => _config.MechanicHatPrefab,
            _ => null
        };
    }
}