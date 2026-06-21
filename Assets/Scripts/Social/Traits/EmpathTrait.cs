using UnityEngine;

public class EmpathTrait : ITrait
{
    public TraitPhase Phase => TraitPhase.ModifyOutcome;

    public bool CheckCondition(SocialContext context, PassengerController owner)
    {
        if (!context.ConflictStarted || context.ConflictResolved || context.Victim != null) return false;

        float sacrificeChance = 0.30f;
        return Random.value < sacrificeChance;
    }

    public string Do(SocialContext context, PassengerController owner)
    {
        context.Victim = owner;
        return $"{owner.GetData.Name} sacrificed themselves!";
    }
}