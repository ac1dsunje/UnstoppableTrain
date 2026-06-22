using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SocialEventManager : MonoBehaviour
{
    [SerializeField] private float _messageDelay = 1.2f;

    public event Action<string> OnMessageGenerated;
    public event Action OnPhaseFinished;

    public bool TryStartSocialPhase(List<PassengerController> passengers)
    {
        var context = new SocialContext
        {
            AllPassengers = passengers
        };

        string message = ExecutePhase(context, TraitPhase.Initiate);

        if (string.IsNullOrEmpty(message))
        {
            return false;
        }

        StartCoroutine(SocialPhaseCoroutine(context, message));
        return true;
    }

    private IEnumerator SocialPhaseCoroutine(SocialContext context, string firstMessage)
    {
        yield return null;

        OnMessageGenerated?.Invoke(firstMessage);
        yield return new WaitForSeconds(_messageDelay);

        var resolvePassengers = context.AllPassengers
            .Where(p => p.TraitBehavior.Phase == TraitPhase.Resolve)
            .ToList();

        string message = ExecutePhaseForPassengers(context, resolvePassengers);

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
            var validVictims = context.AllPassengers;

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