using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EpidemicEventManager : MonoBehaviour
{
    [SerializeField] private float _messageDelay = 1.2f;

    public event Action<string> OnMessageGenerated;
    public event Action OnPhaseFinished;

    public void StartEpidemicPhase(List<PassengerController> passengers)
    {
        StartCoroutine(EpidemicCoroutine(passengers));
    }

    private IEnumerator EpidemicCoroutine(List<PassengerController> passengers)
    {
        yield return null;

        OnMessageGenerated?.Invoke("An epidemic has broken out in the cabin!");
        yield return new WaitForSeconds(_messageDelay);

        int infectedCount = UnityEngine.Random.Range(1, passengers.Count + 1);
        var infected = passengers
            .OrderBy(_ => UnityEngine.Random.value)
            .Take(infectedCount)
            .ToList();

        var availableDoctors = passengers
            .Where(p => p.GetData.role == Role.Doctor)
            .ToList();

        var context = new DisasterContext { AllPassengers = passengers };

        foreach (var patient in infected)
        {
            if (availableDoctors.Count > 0)
            {
                availableDoctors.RemoveAt(0);
                context.Healed.Add(patient);
                OnMessageGenerated?.Invoke($"{patient.GetData.Name} was healed by a doctor.");
            }
            else
            {
                context.Victims.Add(patient);
                OnMessageGenerated?.Invoke($"{patient.GetData.Name} died from the disease.");
            }

            yield return new WaitForSeconds(_messageDelay);
        }

        foreach (var victim in context.Victims)
        {
            victim.Kill();
        }

        OnPhaseFinished?.Invoke();
    }
}