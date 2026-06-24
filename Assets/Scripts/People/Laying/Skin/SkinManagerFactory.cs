using UnityEngine;

public static class SkinManagerFactory
{
    public static ISkinApplier Create(ManSkinConfigSO config, MeshRenderer shape)
    {
        switch (config.SkinManagerType)
        {
            case SkinManagerType.Default:
                return new SkinManager(shape, config);
            default:
                throw new System.ArgumentOutOfRangeException();
        }
    }
}