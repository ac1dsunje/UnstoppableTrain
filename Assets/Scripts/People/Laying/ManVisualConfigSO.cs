using UnityEngine;

[CreateAssetMenu(fileName = "ManVisualConfig", menuName = "Game/Man/Visual Config")]
public class ManVisualConfigSO : ScriptableObject
{
    [field: SerializeField] public GameObject LayingManPrefab { get; private set; }
}