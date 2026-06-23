using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Vector3 _followingOffset = new Vector3(0, 5, -10);
    [SerializeField] private Vector3 _birdOffset = new Vector3(0, 15, 0);
    [SerializeField] private float _time = 0.5f;

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
        Vector3 targetPosition = _target.position + _followingOffset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, _time);
        transform.rotation = Quaternion.Slerp(transform.rotation, _defaultRotation, _time);
    }

    private void BirdView()
    {
        Vector3 targetPosition = _target.position + _birdOffset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, _time);

        Quaternion birdViewRotation = Quaternion.Euler(90f, 0f, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, birdViewRotation, _time);
    }

    public void SetBirdView() => _isFollowingPlayer = false;
    public void SetFollowView() => _isFollowingPlayer = true;
}