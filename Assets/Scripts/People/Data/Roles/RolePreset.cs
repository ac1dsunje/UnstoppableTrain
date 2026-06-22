using UnityEngine;

[CreateAssetMenu(fileName = "RolePreset", menuName = "Game/Role Preset")]
public class RolePreset : ScriptableObject
{
    [field: SerializeField] public RoleWeight[] Weights { get; private set; }
}