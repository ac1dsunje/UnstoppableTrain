using UnityEngine;

public class PsychopathTraitBehaviour : ITraitBehaviour
{
    private readonly TraitConfig _config;

    public PsychopathTraitBehaviour(TraitConfig config)
    {
        _config = config;
    }

    public TraitPhase Phase => TraitPhase.Initiate;

    public bool CheckCondition(SocialContext context, PassengerController owner)
    {
        if (context.ConflictStarted) return false;

        int wallflowers = context.GetCount(Trait.Wallflower);
        float chance = wallflowers * _config.ScaleChance + _config.BaseChance;
        return Random.value < chance;
    }

    public string Do(SocialContext context, PassengerController owner)
    {
        context.ConflictStarted = true;
        return $"{owner.GetData.Name} started a conflict!";
    }
}