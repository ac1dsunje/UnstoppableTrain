using UnityEngine;

[CreateAssetMenu(fileName = "RolePreset", menuName = "Game/Role Preset")]
public class RolePreset : ScriptableObject
{
    public RoleWeight[] Weights;
}