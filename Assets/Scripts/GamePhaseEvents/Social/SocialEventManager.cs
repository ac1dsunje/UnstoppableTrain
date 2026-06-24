using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SocialEventManager : PhaseManagerBase
{
    public SocialEventManager(MonoBehaviour coroutineRunner, float messageDelay) : base(coroutineRunner, messageDelay) { }

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
        yield return TryExecuteAndShowMessage(firstMessage);

        var resolvePassengers = context.AllPassengers
            .Where(p => p.TraitBehavior.Phase == TraitPhase.Resolve)
            .ToList();

        string message = ExecutePhaseForPassengers(context, resolvePassengers);

        if (!string.IsNullOrEmpty(message))
        {
            yield return TryExecuteAndShowMessage(message);
            FinishPhase();
            yield break;
        }
        else
        {
            bool hasLeaders = resolvePassengers.Count > 0;
            if (hasLeaders)
            {
                SendPhaseMessage("Leaders couldn't stop the conflict!");
                yield return new WaitForSeconds(_messageDelay);
            }
            else
            {
                SendPhaseMessage("No one could stop the conflict..");
                yield return new WaitForSeconds(_messageDelay);
            }
        }

        message = ExecutePhase(context, TraitPhase.ModifyOutcome);
        yield return TryExecuteAndShowMessage(message);

        if (context.Victim == null)
        {
            var validVictims = context.AllPassengers;
            if (validVictims.Count > 0)
            {
                var victim = validVictims[Random.Range(0, validVictims.Count)];
                context.Victim = victim;
                SendPhaseMessage($"{victim.GetData.Name} was killed!");
                yield return new WaitForSeconds(_messageDelay);
            }
        }

        if (context.Victim != null)
        {
            context.Victim.Kill();
        }

        FinishPhase();
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

    private IEnumerator TryExecuteAndShowMessage(string message)
    {
        if (!string.IsNullOrEmpty(message))
        {
            SendPhaseMessage(message);
            yield return new WaitForSeconds(_messageDelay);
        }
    }
}