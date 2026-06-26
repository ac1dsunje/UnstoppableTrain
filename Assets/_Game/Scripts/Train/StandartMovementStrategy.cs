using UnityEngine;

public class StandardMovementStrategy : ITrainMovementStrategy
{
    private readonly float _switchRailsSpeed;

    private Transform _transform;
    private Rigidbody _rigidBody;
    private ITrainDataProvider _dataProvider;
    private GameStateManager _gameStateManager;

    private bool _isMovingToSide = false;
    private Vector3 _targetPos = Vector3.zero;
    public StandardMovementStrategy(float switchRailsSpeed = 5f)
    {
        _switchRailsSpeed = switchRailsSpeed;
    }

    public void Initialize(Transform transform, Rigidbody rigidbody, ITrainDataProvider dataProvider)
    {
        _transform = transform;
        _rigidBody = rigidbody;
        _dataProvider = dataProvider;
    }

    public void BindInput(GameStateManager gameStateManager)
    {
        _gameStateManager = gameStateManager;
        _gameStateManager.OnMoveLeft += MoveLeft;
        _gameStateManager.OnMoveRight += MoveRight;
    }

    public void UnbindInput()
    {
        _gameStateManager.OnMoveLeft -= MoveLeft;
        _gameStateManager.OnMoveRight -= MoveRight;
    }
    public void FixedTick(float fixedDeltaTime)
    {
        if (_isMovingToSide) return;
        _rigidBody.linearVelocity = _transform.forward * _dataProvider.GetSpeed();
    }
    public void Tick(float deltaTime)
    {
        if (!_isMovingToSide) return;

        _transform.position = Vector3.MoveTowards(_transform.position, _targetPos, _switchRailsSpeed * deltaTime);

        if (_transform.position == _targetPos)
            _isMovingToSide = false;
    }

    public void MoveLeft()
    {
        var currentRoad = _dataProvider.GetCurrentRoad();
        if (currentRoad == null || currentRoad.IsLeftActive) return;

        _targetPos = new Vector3(currentRoad.LeftRail.transform.position.x, _transform.position.y, _transform.position.z);
        _isMovingToSide = true;
    }

    public void MoveRight()
    {
        var currentRoad = _dataProvider.GetCurrentRoad();
        if (currentRoad == null || currentRoad.IsRightActive) return;

        _targetPos = new Vector3(currentRoad.RightRail.transform.position.x, _transform.position.y, _transform.position.z);
        _isMovingToSide = true;
    }
}