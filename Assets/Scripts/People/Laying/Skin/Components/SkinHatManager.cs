using UnityEngine;

public class SkinHatManager : ISkinComponent
{
    private readonly MeshRenderer _shape;
    private readonly ManSkinConfigSO _config;

    public SkinHatManager(MeshRenderer shape, ManSkinConfigSO config)
    {
        _shape = shape;
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
                Debug.Log("Apply NoSkill hat");
                break;
            case Role.Doctor:
                Debug.Log("Apply Doctor hat");
                break;
            case Role.Driver:
                Debug.Log("Apply Driver hat");
                break;
            case Role.Mechanic:
                Debug.Log("Apply Mechanic hat");
                break;
        }
    }
}