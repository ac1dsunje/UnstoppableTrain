using UnityEngine;

[CreateAssetMenu(fileName = "New Train Data", menuName = "Game/Train/Data")]

public class TrainDataSO : ScriptableObject
{
    [field: SerializeField] public float MoveSpeed { get; private set; }
    [field: SerializeField] public int MaxAmount { get; private set; }
}