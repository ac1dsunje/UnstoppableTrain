using UnityEngine;

public class SkinManager
{
    private readonly MeshRenderer _shape;
    private readonly ManSkinConfigSO _config;

    public SkinManager (MeshRenderer shape, ManSkinConfigSO config)
    {
        _shape = shape;
        _config = config;
    }

    public void ApplySkin(ManData data)
    {
        ApplyMaterial(data.trait);
        ApplyHat(data.role);
    }

    private void ApplyMaterial(Trait trait)
    {
        Material mat = null;
        Material current = _shape.material;

        switch (trait)
        {
            case Trait.Wallflower:
                mat = _config.WallflowerMat;
                break;
            case Trait.Psychopath:
                mat = _config.PsychopathMat;
                break;
            case Trait.Leader:
                mat = _config.LeaderMat;
                break;
            case Trait.Empath:
                mat = _config.EmpathMat;
                break;
        }

        _shape.material = mat ?? current;
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