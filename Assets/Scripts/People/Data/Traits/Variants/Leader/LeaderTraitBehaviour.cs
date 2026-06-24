using Unity.VisualScripting;
using UnityEngine;

public class LeaderTraitBehaviour : ITraitBehaviour
{
    private readonly TraitConfig _config;

    public LeaderTraitBehaviour(TraitConfig config)
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
        return $"{owner.GetData.Name} managed to stop the conflict!";
    }
}