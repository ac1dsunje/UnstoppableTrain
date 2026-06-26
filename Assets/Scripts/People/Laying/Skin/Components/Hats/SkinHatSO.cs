using UnityEngine;

[CreateAssetMenu(fileName = "ManSkinHatConfig", menuName = "Game/Man/Skin/Hat Config")]
public class SkinHatSO : ScriptableObject
{
    [field: SerializeField] public GameObject DriverHatPrefab { get; private set; }
    [field: SerializeField] public GameObject DoctorHatPrefab { get; private set; }
    [field: SerializeField] public GameObject MechanicHatPrefab { get; private set; }
}