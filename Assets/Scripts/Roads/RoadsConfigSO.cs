using UnityEngine;

[CreateAssetMenu(fileName = "RoadsConfig", menuName = "Game/Roads/General Config")]
public class RoadsConfigSO : ScriptableObject
{
    [field: SerializeField] public int MaxRoads { get; private set; }
    [field: SerializeField] public GameObject RoadPrefab { get; private set; }
    [field: SerializeField] public RoadSegmentConfigSO[] SegmentConfigs { get; private set; }
}