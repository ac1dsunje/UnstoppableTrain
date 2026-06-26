using UnityEngine;

[CreateAssetMenu(fileName = "StationsPreset", menuName = "Game/Stations Preset")]
public class StationsPreset : ScriptableObject
{
    [field: SerializeField] public StationsRange Range { get; private set; }
}