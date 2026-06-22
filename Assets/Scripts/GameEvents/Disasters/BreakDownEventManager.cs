using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakdownEventManager : MonoBehaviour
{
    [SerializeField] private float _messageDelay = 1.2f;

    public event Action<string> OnMessageGenerated;
    public event Action OnPhaseFinished;

    public void StartBreakdownPhase(List<PassengerController> passengers)
    {
        StartCoroutine(BreakdownCoroutine(passengers));
    }

    private IEnumerator BreakdownCoroutine(List<PassengerController> passengers)
    {
        yield return null;

        OnMessageGenerated?.Invoke("The engine has broken down!");
        yield return new WaitForSeconds(_messageDelay);

        var mechanic = RoleFactory.FindFirst(passengers, Role.Mechanic);

        if (mechanic != null)
        {
            OnMessageGenerated?.Invoke($"{mechanic.GetData.Name} successfully repaired the engine!");
            yield return new WaitForSeconds(_messageDelay);
        }
        else
        {
            OnMessageGenerated?.Invoke("No mechanic on board! The train is stuck...");
            yield return new WaitForSeconds(_messageDelay);
        }

        OnPhaseFinished?.Invoke();
    }
}