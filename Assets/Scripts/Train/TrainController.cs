using UnityEngine;

public class TrainController: MonoBehaviour, Imovement
{
    [SerializeField] private TrainSO _data;

    public float GetSpeed()
    {
        return _data.MoveSpeed;
    }
}