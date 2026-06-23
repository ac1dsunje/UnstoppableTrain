using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakdownEventManager : PhaseManagerBase
{
    public BreakdownEventManager(MonoBehaviour coroutineRunner, float messageDelay) : base(coroutineRunner, messageDelay) { }

    public void StartBreakdownPhase(List<PassengerController> passengers)
    {
        StartCoroutine(BreakdownCoroutine(passengers));
    }

    private IEnumerator BreakdownCoroutine(List<PassengerController> passengers)
    {
        yield return null;

        SendPhaseMessage("The engine has broken down!");
        yield return new WaitForSeconds(_messageDelay);

        var mechanic = RoleFactory.FindFirst(passengers, Role.Mechanic);

        if (mechanic != null)
        {
            SendPhaseMessage($"{mechanic.GetData.Name} successfully repaired the engine!");
            yield return new WaitForSeconds(_messageDelay);
        }
        else
        {
            SendPhaseMessage("No mechanic on board! The train is stuck...\nPassengers need more time to fix the train...");
            yield return new WaitForSeconds(_messageDelay * 3);
        }

        FinishPhase();
    }
}