using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakdownEventManager : PhaseManagerBase
{
    private float _timeScaler = 5f;
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

        int mechanicCount = RoleFactory.CountRole(passengers, Role.Mechanic);
        float repairDelay = (_messageDelay * _timeScaler) / (mechanicCount > 0 ? mechanicCount + 1 : 1);

        string mechanicsWord = mechanicCount == 1 ? "mechanic" : "mechanics";
        string repairTimeText = repairDelay.ToString("0.0");
        SendPhaseMessage($"We have {mechanicCount} {mechanicsWord} on board.\nRepair will take {repairTimeText} seconds.");

        yield return new WaitForSeconds(repairDelay);

        SendPhaseMessage("The engine was succesfully repaired");
        yield return new WaitForSeconds(_messageDelay);

        FinishPhase();
    }
}