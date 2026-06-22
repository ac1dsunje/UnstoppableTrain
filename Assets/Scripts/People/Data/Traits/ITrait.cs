using UnityEngine;

public enum TraitPhase
{
    Initiate,
    Resolve,
    ModifyOutcome
}

public interface ITrait
{
    TraitPhase Phase { get; }
    bool CheckCondition(TraitContext context, PassengerController owner);
    string Do(TraitContext context, PassengerController owner);
}