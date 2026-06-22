public class WallflowerTrait : ITrait
{
    public TraitPhase Phase => TraitPhase.Initiate;

    public bool CheckCondition(TraitContext context, PassengerController owner)
    {
        return false;
    }

    public string Do(TraitContext context, PassengerController owner)
    {
        return "";
    }
}