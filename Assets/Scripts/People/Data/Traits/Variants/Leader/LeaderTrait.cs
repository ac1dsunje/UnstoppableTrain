using UnityEngine;

public class LeaderTrait : ITrait
{
    private readonly LeaderConfig _config;

    public LeaderTrait(LeaderConfig config)
    {
        _config = config;
    }

    public TraitPhase Phase => TraitPhase.Resolve;

    public bool CheckCondition(SocialContext context, PassengerController owner)
    {
        if (!context.ConflictStarted || context.ConflictResolved) return false;

        int empaths = context.GetCount(Trait.Empath);
        float chance = empaths * _config.ScaleChance;
        return Random.value < chance;
    }

    public string Do(SocialContext context, PassengerController owner)
    {
        context.ConflictResolved = true;
        return "Leaders managed to stop the conflict!";
    }
}