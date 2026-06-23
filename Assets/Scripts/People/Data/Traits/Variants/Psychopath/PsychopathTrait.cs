using UnityEngine;

public class PsychopathTrait : ITrait
{
    private readonly PsychopathConfig _config;

    public PsychopathTrait(PsychopathConfig config)
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
        return "A Psychopath started a conflict!";
    }
}