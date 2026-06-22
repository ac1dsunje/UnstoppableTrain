public class WallflowerTrait : ITrait
{
    public TraitPhase Phase => TraitPhase.Initiate;

    public bool CheckCondition(SocialContext context, PassengerController owner)
    {
        return false;
    }

    public string Do(SocialContext context, PassengerController owner)
    {
        return "";
    }
}