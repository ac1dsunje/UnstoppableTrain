using UnityEngine;

public class SkinMaterialManager : ISkinComponent
{
    private readonly MeshRenderer _shape;
    private readonly ManSkinConfigSO _config;

    public SkinMaterialManager(MeshRenderer shape, ManSkinConfigSO config)
    {
        _shape = shape;
        _config = config;
    }

    public void Apply(ManData data)
    {
        ApplyMaterial(data.trait);
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
}