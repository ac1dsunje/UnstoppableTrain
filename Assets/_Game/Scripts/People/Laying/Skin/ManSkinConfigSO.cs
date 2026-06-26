using UnityEngine;

using System;

[Flags]
public enum SkinComponentType
{
    None,
    Material,
    Hat,
}

[CreateAssetMenu(fileName = "ManSkinConfig", menuName = "Game/Man/Skin/General Config")]
public class ManSkinConfigSO : ScriptableObject
{
    [field: SerializeField] public SkinComponentType SkinComponents { get; private set; }

    [field: SerializeField] public SkinMaterialSO MaterialConfig { get; private set; }

    [field: SerializeField] public SkinHatSO HatsConfig { get; private set; }
}