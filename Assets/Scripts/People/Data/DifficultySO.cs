
using UnityEngine;

[CreateAssetMenu(fileName = "DifficultySO", menuName = "Game/Difficulty")]

public class DifficultySO : ScriptableObject
{
    [field: SerializeField] public TraitPreset traitLevel { get; private set; }

    [field: SerializeField] public RolePreset roleLevel { get; private set; }

    [field: SerializeField] public StationsPreset stationsLevel { get; private set; }
}
