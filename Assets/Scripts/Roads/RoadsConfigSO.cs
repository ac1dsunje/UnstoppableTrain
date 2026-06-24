using UnityEngine;


[CreateAssetMenu(fileName = "RoadsConfig", menuName = "Game/Roads/General Config")]
public class RoadsConfigSO : ScriptableObject
{
    [field: SerializeField] public int ChoosingRoadChance { get; private set; }
    [field: SerializeField] public int StationRoadChance { get; private set; }
    [field: SerializeField] public int MaxRoads { get; private set; }

    [field: SerializeField] public GameObject MovingRoadPrefab { get; private set; }
    [field: SerializeField] public GameObject ChoosingRoadPrefab { get; private set; }
    [field: SerializeField] public GameObject StationRoadPrefab { get; private set; }
}