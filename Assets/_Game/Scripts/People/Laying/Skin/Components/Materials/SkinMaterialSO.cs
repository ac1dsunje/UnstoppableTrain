using UnityEngine;

[CreateAssetMenu(fileName = "ManSkinMaterialConfig", menuName = "Game/Man/Skin/Material Config")]
public class SkinMaterialSO : ScriptableObject
{
    [field: SerializeField] public Material WallflowerMat { get; private set; }
    [field: SerializeField] public Material PsychopathMat { get; private set; }
    [field: SerializeField] public Material LeaderMat { get; private set; }
    [field: SerializeField] public Material EmpathMat { get; private set; }
}