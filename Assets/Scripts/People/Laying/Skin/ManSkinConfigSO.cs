using UnityEngine;

[CreateAssetMenu(fileName = "ManSkinConfig", menuName = "Game/Man/Skin Config")]
public class ManSkinConfigSO : ScriptableObject
{
    [field: SerializeField] public Material WallflowerMat { get; private set; }
    [field: SerializeField] public Material PsychopathMat { get; private set; }
    [field: SerializeField] public Material LeaderMat { get; private set; }
    [field: SerializeField] public Material EmpathMat { get; private set; }
}