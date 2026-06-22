using UnityEngine;

[CreateAssetMenu(fileName = "TraitPreset", menuName = "Game/Trait Preset")]
public class TraitPreset : ScriptableObject
{
    public TraitWeight[] Weights;
}