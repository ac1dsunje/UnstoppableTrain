using System;
using UnityEngine;
using System.Collections;

public abstract class PhaseManagerBase
{
    protected readonly MonoBehaviour _coroutineRunner;
    protected readonly float _messageDelay;

    public event Action<string> OnMessageGenerated;
    public event Action OnPhaseFinished;

    protected PhaseManagerBase(MonoBehaviour coroutineRunner, float messageDelay)
    {
        _coroutineRunner = coroutineRunner;
        _messageDelay = messageDelay;
    }

    protected void SendPhaseMessage(string message)
    {
        OnMessageGenerated?.Invoke(message);
    }

    protected void FinishPhase()
    {
        OnPhaseFinished?.Invoke();
    }

    protected Coroutine StartCoroutine(IEnumerator routine)
    {
        return _coroutineRunner.StartCoroutine(routine);
    }
}