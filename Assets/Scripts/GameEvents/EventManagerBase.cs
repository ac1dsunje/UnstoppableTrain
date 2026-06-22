using System;
using UnityEngine;

public abstract class EventManagerBase : MonoBehaviour
{
    [SerializeField] protected float _messageDelay = 1.2f;

    public event Action<string> OnMessageGenerated;
    public event Action OnPhaseFinished;

    protected void SendPhaseMessage(string message)
    {
        OnMessageGenerated?.Invoke(message);
    }

    protected void FinishPhase()
    {
        OnPhaseFinished?.Invoke();
    }
}