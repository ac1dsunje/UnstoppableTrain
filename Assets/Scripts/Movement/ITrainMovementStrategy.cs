using UnityEngine;

public interface ITrainMovementStrategy
{
    void Initialize(Transform transform, Rigidbody rigidbody, ITrainDataProvider dataProvider);
    void BindInput(GameStateManager gameStateManager);
    void UnbindInput();

    void Tick(float deltaTime);
    void FixedTick(float fixedDeltaTime);

    void MoveLeft();
    void MoveRight();
}