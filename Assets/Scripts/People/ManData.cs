
using System;

public enum Role
{
    Driver,
    Mechanic,
    Doctor,
    NoSkill
}

[Serializable]
public class ManData
{
    public string Name;
    public Role role;
    public Trait trait;
    public int StationsNeeded; // how many chunks this passenger is going to stay in the train
    public int StationsLeft; // before passenger leaves
}