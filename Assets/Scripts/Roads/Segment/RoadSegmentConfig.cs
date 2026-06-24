using UnityEngine;


[CreateAssetMenu(fileName = "RoadSegmentConfig", menuName = "Game/Roads/Segment Config")]
public class RoadSegmentConfigSO : ScriptableObject
{

    [field: SerializeField] public float roadLength { get; private set; } = 10f;
    [field: SerializeField] public GameObject RailPrefab { get; private set; }
    [field: SerializeField] public EnvironmentAtlas _environmentAtlas;
    [field: SerializeField] public int _maxMenOnTheRail { get; private set; } = 3;
    [field: SerializeField] public SoundData _onEnterSound { get; private set; }
}