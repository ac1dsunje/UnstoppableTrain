public class WallflowerTraitBehaviour : ITraitBehaviour
{
    private readonly TraitConfig _config;

    public WallflowerTraitBehaviour(TraitConfig config)
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