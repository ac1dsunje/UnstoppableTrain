using UnityEngine;


public class TrainMovement : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    private Imovement _movementController;
    private Rigidbody _rigidBody;

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
        _movementController = GetComponent<Imovement>();
    }

    private void FixedUpdate()
    {
        MoveForward();
    }

    private void MoveLeft()
    {
        if (_movementController.GetCurrentRoad().IsLeftActive) return;
        // ToDo : move train to the left rail & some animations?
    }

    private void MoveRight()
    {
        if (_movementController.GetCurrentRoad().IsRightActive) return;
        // ToDo : move train to the right rail & some animations?
    }

    private void MoveForward()
    {
        _rigidBody.linearVelocity = transform.forward * _movementController.GetSpeed();
    }
}