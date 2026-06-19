
using System;

public enum Role
{
    Driver,
    Mechanic,
    Doctor,
    NoSkill
}

public enum Trait
{
    Wallflower,
    Leader,
    Psychopath,
    Empath
}

[Serializable]
public class ManData
{
    public Role role;
    public Trait trait;
}