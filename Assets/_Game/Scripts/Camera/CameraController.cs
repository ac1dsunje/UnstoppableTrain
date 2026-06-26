using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CameraConfigSO _config;

    private Transform _target;
    private bool _isFollowingPlayer;
    private Quaternion _defaultRotation;

    public CameraController Initialize(Transform target)
    {
        _target = target;
        _defaultRotation = transform.rotation;
        SetFollowView();
        return this;
    }

    private void LateUpdate()
    {
        if (_target == null) return;

        if (_isFollowingPlayer) FollowPlayer();
        else BirdView();
    }

    private void FollowPlayer()
    {
        Vector3 targetPosition = _target.position + _config.FollowingOffset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, _config.Time);
        transform.rotation = Quaternion.Slerp(transform.rotation, _defaultRotation, _config.Time);
    }

    private void BirdView()
    {
        Vector3 targetPosition = _target.position + _config.BirdOffset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, _config.Time);

        Quaternion birdViewRotation = Quaternion.Euler(90f, 0f, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, birdViewRotation, _config.Time);
    }

    public void SetBirdView() => _isFollowingPlayer = false;
    public void SetFollowView() => _isFollowingPlayer = true;
}