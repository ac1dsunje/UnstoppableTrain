using UnityEngine;

public class SkinHatManager : ISkinComponent
{
    private readonly Transform _hatHolder;
    private readonly ManSkinConfigSO _config;

    public SkinHatManager(Transform hatHolder, ManSkinConfigSO config)
    {
        _hatHolder = hatHolder;
        _config = config;
    }

    public void Apply(ManData data)
    {
        ApplyHat(data.role);
    }

    private void ApplyHat(Role role)
    {
        switch (role)
        {
            case Role.NoSkill:
                break;
            case Role.Doctor:
                Object.Instantiate(_config.DoctorHatPrefab, _hatHolder);
        break;
            case Role.Driver:
                Object.Instantiate(_config.DriverHatPrefab, _hatHolder);
                break;
            case Role.Mechanic:
                Object.Instantiate(_config.MechanicHatPrefab, _hatHolder);
                break;
        }
    }
}