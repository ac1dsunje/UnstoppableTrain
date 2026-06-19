using UnityEngine;


public class TrainMovement : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    private Imovement _controller;
    private Rigidbody _rigidBody;
    private RoadController _currentRoad;

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

    private void MoveForward()
    {
        _rigidBody.linearVelocity = transform.forward * _controller.GetSpeed();
    }

    private void MoveLeft()
    {
        _currentRoad = _controller.GetCurrentRoad();
        if (_currentRoad.IsLeftActive) return;

        transform.position = new Vector3(_currentRoad.LeftRail.transform.position.x, transform.position.y, transform.position.z);
        // some animations?
    }

    private void MoveRight()
    {
        _currentRoad = _controller.GetCurrentRoad();
        if (_currentRoad.IsRightActive) return;

        transform.position = new Vector3(_currentRoad.RightRail.transform.position.x, transform.position.y, transform.position.z);
        // some animations?
    }
}