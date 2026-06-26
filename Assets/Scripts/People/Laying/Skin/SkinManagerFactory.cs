using System.Collections.Generic;
using UnityEngine;

public static class SkinManagerFactory
{
    public static ISkinComponent Create(ManSkinConfigSO config, MeshRenderer shape, Transform hatHolder)
    {
        var components = new List<ISkinComponent>();

        if (config.SkinComponents.HasFlag(SkinComponentType.Material))
        {
            components.Add(new SkinMaterialManager(shape, config));
        }

        if (config.SkinComponents.HasFlag(SkinComponentType.Hat))
        {
            components.Add(new SkinHatManager(hatHolder, config));
        }

        if (components.Count == 0)
        {
            return new EmptySkinComponent();
        }

        if (components.Count == 1)
        {
            return components[0];
        }

        return new CompositeSkinManager(components);
    }
}