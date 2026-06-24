public enum TraitPhase
{
    Initiate,
    Resolve,
    ModifyOutcome
}

public interface ITraitBehaviour
{
    TraitPhase Phase { get; }
    bool CheckCondition(SocialContext context, PassengerController owner);
    string Do(SocialContext context, PassengerController owner);
}