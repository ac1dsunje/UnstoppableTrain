
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
    public int StationsNeeded;
    public int StationsLeft;
}