using UnityEngine;

[CreateAssetMenu(fileName = "ManSoundConfig", menuName = "Game/Man/Sound Config")]
public class ManSoundConfigSO : ScriptableObject
{
    [field: SerializeField] public SoundData OnDeathSound { get; private set; }
}