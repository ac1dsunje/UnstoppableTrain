using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SocialManager : MonoBehaviour
{
    [SerializeField] private TrainController _train;
    [SerializeField] private float _messageDelay = 1.2f;

    public event Action<string> OnMessageGenerated;
    public event Action OnPhaseFinished;

    public void StartSocialPhase()
    {
        StartCoroutine(SocialPhaseCoroutine());
    }

    private IEnumerator SocialPhaseCoroutine()
    {
        var context = new SocialContext
        {
            AllPassengers = new List<PassengerController>(_train.GetPassengers())
        };

        string message = ExecutePhase(context, TraitPhase.Initiate);

        if (string.IsNullOrEmpty(message))
        {
            OnMessageGenerated?.Invoke("The journey was peaceful.");
            yield return new WaitForSeconds(_messageDelay);
            OnPhaseFinished?.Invoke();
            yield break;
        }

        OnMessageGenerated?.Invoke(message);
        yield return new WaitForSeconds(_messageDelay);

        var resolvePassengers = context.AllPassengers
            .Where(p => p.TraitBehavior.Phase == TraitPhase.Resolve)
            .ToList();

        message = ExecutePhaseForPassengers(context, resolvePassengers);

        if (!string.IsNullOrEmpty(message))
        {
            OnMessageGenerated?.Invoke(message);
            yield return new WaitForSeconds(_messageDelay);
            OnPhaseFinished?.Invoke();
            yield break;
        }
        else
        {
            OnMessageGenerated?.Invoke("Leaders couldn't stop the conflict!");
            yield return new WaitForSeconds(_messageDelay);
        }

        message = ExecutePhase(context, TraitPhase.ModifyOutcome);

        if (!string.IsNullOrEmpty(message))
        {
            OnMessageGenerated?.Invoke(message);
            yield return new WaitForSeconds(_messageDelay);
        }
        else
        {
            var validVictims = context.AllPassengers
                .Where(p => p.GetData.trait != Trait.Psychopath)
                .ToList();

            if (validVictims.Count > 0)
            {
                var victim = validVictims[UnityEngine.Random.Range(0, validVictims.Count)];
                context.Victim = victim;
                OnMessageGenerated?.Invoke($"{victim.GetData.Name} died!");
                yield return new WaitForSeconds(_messageDelay);
            }
        }

        if (context.Victim != null)
        {
            context.Victim.Kill();
        }

        OnPhaseFinished?.Invoke();
    }

    private string ExecutePhase(SocialContext context, TraitPhase phase)
    {
        var passengers = context.AllPassengers
            .Where(p => p.TraitBehavior.Phase == phase)
            .ToList();
        return ExecutePhaseForPassengers(context, passengers);
    }

    private string ExecutePhaseForPassengers(SocialContext context, List<PassengerController> passengers)
    {
        foreach (var passenger in passengers)
        {
            if (passenger.TraitBehavior.CheckCondition(context, passenger))
            {
                return passenger.TraitBehavior.Do(context, passenger);
            }
        }
        return null;
    }
}