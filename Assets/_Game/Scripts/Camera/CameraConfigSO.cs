using UnityEngine;

[CreateAssetMenu(fileName = "CameraConfig", menuName = "Game/Camera/Movement")]
public class CameraConfigSO: ScriptableObject
{
    [field: SerializeField] public Vector3 FollowingOffset { get; private set; } = new Vector3(0, 5, -5);
    [field: SerializeField] public Vector3 BirdOffset { get; private set; } = new Vector3(0, 7, 9);
    [field: SerializeField] public float Time { get; private set; } = 1f;
}