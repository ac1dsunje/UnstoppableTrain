public class WallflowerTraitBehaviour : ITraitBehaviour
{
    private readonly WallflowerConfig _config;

    public WallflowerTraitBehaviour(WallflowerConfig config)
    {
        _config = config;
    }

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