using UnityEngine;

[CreateAssetMenu(fileName = "TraitsConfig", menuName = "Game/Traits/Traits Config")]
public class TraitsConfigSO : ScriptableObject
{
    public TraitConfig EmpathConfig;
    public TraitConfig LeaderConfig;
    public TraitConfig PsychopathConfig;
    public TraitConfig WallflowerConfig;
}