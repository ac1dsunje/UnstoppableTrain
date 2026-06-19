using UnityEngine;

[CreateAssetMenu(fileName = "New Train Data", menuName = "Train/Train Data")]

public class TrainSO : ScriptableObject
{
    [field: SerializeField] public float MoveSpeed { get; private set; }
    [field: SerializeField] public int MaxAmount { get; private set; }
}