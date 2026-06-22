using UnityEngine;

public class LeaderTrait : ITrait
{
    public TraitPhase Phase => TraitPhase.Resolve;

    public bool CheckCondition(SocialContext context, PassengerController owner)
    {
        if (!context.ConflictStarted || context.ConflictResolved) return false;

        int empaths = context.GetCount(Trait.Empath);
        float chance = empaths * 0.10f;
        return Random.value < chance;
    }

    public string Do(SocialContext context, PassengerController owner)
    {
        context.ConflictResolved = true;
        return "Leaders managed to stop the conflict!";
    }
}