using UnityEngine;

using System;

[Flags]
public enum SkinComponentType
{
    None,
    Material,
    Hat,
}

[CreateAssetMenu(fileName = "ManSkinConfig", menuName = "Game/Man/Skin Config")]
public class ManSkinConfigSO : ScriptableObject
{
    [field: SerializeField] public SkinComponentType SkinComponents { get; private set; }   
    [field: SerializeField] public Material WallflowerMat { get; private set; }
    [field: SerializeField] public Material PsychopathMat { get; private set; }
    [field: SerializeField] public Material LeaderMat { get; private set; }
    [field: SerializeField] public Material EmpathMat { get; private set; }

    [field: SerializeField] public GameObject DriverHatPrefab { get; private set; }
    [field: SerializeField] public GameObject DoctorHatPrefab { get; private set; }
    [field: SerializeField] public GameObject MechanicHatPrefab { get; private set; }
}