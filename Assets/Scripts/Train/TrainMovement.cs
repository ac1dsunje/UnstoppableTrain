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
        Debug.Log("Move left");
    }

    private void MoveRight()
    {
        Debug.Log("Move right");
    }

    private void MoveForward()
    {
        _rigidBody.linearVelocity = transform.forward * _movementController.GetSpeed();
    }
}