using UnityEngine;


public class TrainMovement : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private float _switchRailsSpeed = 5f;
    private Imovement _controller;
    private Rigidbody _rigidBody;
    private RoadController _currentRoad;

    private bool _isMovingToSide = false;
    private Vector3 _targetPos = new();

    private void OnEnable()
    {
        _gameManager.OnMoveLeft += MoveLeft;
        _gameManager.OnMoveRight += MoveRight;
    }

    private void OnDisable()
    {
        _gameManager.OnMoveLeft -= MoveLeft;
        _gameManager.OnMoveRight -= MoveRight;
    }

    private void Awake()
    {

        _rigidBody = GetComponent<Rigidbody>();
        _controller = GetComponent<Imovement>();
    }

    private void FixedUpdate()
    {
        MoveForward();
    }

    private void Update()
    {
        MoveToSide(_targetPos);
    }

    private void MoveForward()
    {
        if (_isMovingToSide) return;

        _rigidBody.linearVelocity = transform.forward * _controller.GetSpeed();
    }

    private void MoveLeft()
    {
        _currentRoad = _controller.GetCurrentRoad();
        if (_currentRoad.IsLeftActive) return;

        _targetPos = new Vector3(_currentRoad.LeftRail.transform.position.x, transform.position.y, transform.position.z);
        _isMovingToSide = true;
    }

    private void MoveRight()
    {
        _currentRoad = _controller.GetCurrentRoad();
        if (_currentRoad.IsRightActive) return;

        _targetPos = new Vector3(_currentRoad.RightRail.transform.position.x, transform.position.y, transform.position.z);
        _isMovingToSide = true;
    }

    private void MoveToSide(Vector3 targetPos)
    {
        if (_isMovingToSide)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, _switchRailsSpeed * Time.deltaTime);
        }
        if (transform.position == targetPos)
        {
            _isMovingToSide = false;
        }
    }
}