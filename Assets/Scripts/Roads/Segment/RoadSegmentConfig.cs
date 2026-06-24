using UnityEngine;

public enum RoadType 
{ 
    Moving, 
    Choosing, 
    Station
}

[CreateAssetMenu(fileName = "RoadSegmentConfig", menuName = "Game/Roads/Segment Config")]
public class RoadSegmentConfigSO : ScriptableObject
{
    [field: SerializeField] public RoadType RoadType { get; private set; }
    [field: SerializeField] public float Weight { get; private set; } = 1f;
    [field: SerializeField] public float RoadLength { get; private set; } = 10f;
    [field: SerializeField] public GameObject RailPrefab { get; private set; }
    [field: SerializeField] public EnvironmentAtlas EnvironmentAtlas { get; private set; }
    [field: SerializeField] public int MaxMenOnTheRail { get; private set; } = 3;
    [field: SerializeField] public SoundData OnEnterSound { get; private set; }
}