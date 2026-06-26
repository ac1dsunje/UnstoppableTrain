using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationManager : PhaseManagerBase
{
    public StationManager(MonoBehaviour coroutineRunner, float messageDelay) : base(coroutineRunner, messageDelay) { }

    public bool TryStartStationPhase(List<PassengerController> passengers)
    {
        if (passengers == null || passengers.Count == 0) return false;

        StartCoroutine(StationCoroutine(passengers));
        return true;
    }

    private IEnumerator StationCoroutine(List<PassengerController> passengers)
    {
        yield return null;

        SendPhaseMessage("Train came to the station");
        yield return new WaitForSeconds(_messageDelay);

        foreach (var passenger in passengers)
        {
            if (passenger.TryLeave())
            {
                SendPhaseMessage($"{passenger.GetData.Name} has left the train");
                yield return new WaitForSeconds(_messageDelay);
            }
        }

        FinishPhase();
    }
}