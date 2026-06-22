using UnityEngine;

public class PsychopathTrait : ITrait
{
    public TraitPhase Phase => TraitPhase.Initiate;

    public bool CheckCondition(SocialContext context, PassengerController owner)
    {
        if (context.ConflictStarted) return false;

        int wallflowers = context.GetCount(Trait.Wallflower);
        float chance = wallflowers * 0.10f + 0.1f;
        return Random.value < chance;
    }

    public string Do(SocialContext context, PassengerController owner)
    {
        context.ConflictStarted = true;
        return "A Psychopath started a conflict!";
    }
}