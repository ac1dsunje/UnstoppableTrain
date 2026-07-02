using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TrainView : MonoBehaviour, ITrainView
{
    private float _switchRailsSpeed;

    private Rigidbody _rb;

    private bool _isMovingToSide = false;
    private Vector3 _targetPos = Vector3.zero;

    private float _speed;

    public TrainView Initialize(float switchRailsSpeed = 5f) 
    {
        _switchRailsSpeed = switchRailsSpeed;

        _rb = GetComponent<Rigidbody>();

        return this;
    }

    private void FixedUpdate()
    {
        if (_isMovingToSide) return;
        _rb.linearVelocity = transform.forward * _speed;
    }

    private void Update()
    {
        if (!_isMovingToSide) return;

        transform.position = Vector3.MoveTowards(transform.position, _targetPos, _switchRailsSpeed * Time.deltaTime);

        if (transform.position.x == _targetPos.x)
            _isMovingToSide = false;
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }

    public void Move(Transform railTransform) 
    {
        _targetPos = new Vector3(railTransform.position.x, transform.position.y, transform.position.z);
        _isMovingToSide = true;
    }
}