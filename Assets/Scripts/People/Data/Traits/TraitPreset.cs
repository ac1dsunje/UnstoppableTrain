using UnityEngine;

[CreateAssetMenu(fileName = "TraitChance", menuName = "Game/Trait Chances")]
public class TraitPreset : ScriptableObject
{
    [field: SerializeField] public TraitWeight[] Weights { get; private set; }
}