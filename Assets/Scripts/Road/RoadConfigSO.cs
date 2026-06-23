using UnityEngine;


[CreateAssetMenu(fileName = "RoadConfig", menuName = "Game/Road Config")]
public class RoadConfigSO : ScriptableObject
{
    [field: SerializeField] public int _choosingRoadChance { get; private set; }
    [field: SerializeField] public int _stationRoadChance { get; private set; }
    [field: SerializeField] public int _maxRoads { get; private set; }

    [field: SerializeField] public GameObject movingRoadPrefab { get; private set; }
    [field: SerializeField] public GameObject choosingRoadPrefab { get; private set; }
    [field: SerializeField] public GameObject stationRoadPrefab { get; private set; }
}