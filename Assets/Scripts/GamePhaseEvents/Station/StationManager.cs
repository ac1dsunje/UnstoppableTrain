using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationManager : PhaseManagerBase
{
    public void StartStationPhase(List<PassengerController> passengers)
    {
        StartCoroutine(StationCoroutine(passengers));
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

        SendPhaseMessage("Leaving station..");
        yield return new WaitForSeconds(_messageDelay);

        FinishPhase();
    }
}