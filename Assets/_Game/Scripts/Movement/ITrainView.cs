using UnityEngine;

public interface ITrainView
{
    void SetSpeed(float speed);
    void Move(Transform railTransform);
}