using UnityEngine;

public class EmpathTraitBehaviour : ITraitBehaviour
{
    private readonly EmpathConfig _config;

    public EmpathTraitBehaviour(EmpathConfig config)
    {
        _config = config;
    }

    public TraitPhase Phase => TraitPhase.ModifyOutcome;

    public bool CheckCondition(SocialContext context, PassengerController owner)
    {
        if (!context.ConflictStarted || context.ConflictResolved || context.Victim != null) return false;

        return Random.value < _config.SacrificeChance;
    }

    public string Do(SocialContext context, PassengerController owner)
    {
        context.Victim = owner;
        return $"{owner.GetData.Name} sacrificed themselves!";
    }
}