using UnityEngine;

[CreateAssetMenu(fileName = "TraitsConfig", menuName = "Game/Traits/Traits Config")]
public class TraitsConfigSO : ScriptableObject
{
    public EmpathConfig EmpathConfig;
    public LeaderConfig LeaderConfig;
    public PsychopathConfig PsychopathConfig;
    public WallflowerConfig WallflowerConfig;
}