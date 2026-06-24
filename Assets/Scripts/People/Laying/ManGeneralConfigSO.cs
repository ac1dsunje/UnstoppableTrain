using UnityEngine;

[CreateAssetMenu(fileName = "ManGeneralConfig", menuName = "Game/Man/General Config")]
public class ManGeneralConfigSO : ScriptableObject
{
    [field: SerializeField] public ManSoundConfigSO ManConfig { get; private set; }
    [field: SerializeField] public ManSkinConfigSO SkinConfig { get; private set; }
}