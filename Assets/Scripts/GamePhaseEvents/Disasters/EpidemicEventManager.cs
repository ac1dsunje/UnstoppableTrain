using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EpidemicEventManager : PhaseManagerBase
{
    public void StartEpidemicPhase(List<PassengerController> passengers)
    {
        StartCoroutine(EpidemicCoroutine(passengers));
    }

    private IEnumerator EpidemicCoroutine(List<PassengerController> passengers)
    {
        yield return null;

        SendPhaseMessage("An epidemic has broken out in the cabin!");
        yield return new WaitForSeconds(_messageDelay);

        int infectedCount = Random.Range(1, passengers.Count + 1);
        var infected = passengers
            .OrderBy(_ => Random.value)
            .Take(infectedCount)
            .ToList();

        var availableDoctors = passengers
            .Where(p => p.GetData.role == Role.Doctor)
            .ToList();

        var context = new EpidemicContext { AllPassengers = passengers };

        foreach (var patient in infected)
        {
            if (availableDoctors.Count > 0)
            {
                availableDoctors.RemoveAt(0);
                context.Healed.Add(patient);
                SendPhaseMessage($"{patient.GetData.Name} was healed by a doctor.");
            }
            else
            {
                context.Victims.Add(patient);
                SendPhaseMessage($"{patient.GetData.Name} died from the disease.");
            }

            yield return new WaitForSeconds(_messageDelay);
        }

        foreach (var victim in context.Victims)
        {
            victim.Kill();
        }

        FinishPhase();
    }
}