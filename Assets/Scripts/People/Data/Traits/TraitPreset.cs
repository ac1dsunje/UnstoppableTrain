using UnityEngine;

[CreateAssetMenu(fileName = "TraitPreset", menuName = "Game/Trait Preset")]
public class TraitPreset : ScriptableObject
{
    [field: SerializeField] public TraitWeight[] Weights { get; private set; }
}