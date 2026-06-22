public static class ManFactory
{

    public static ManData Create(
    string name = null,
    Role? role = null,
    Trait? trait = null,
    int? stationsNeeded = null)
    {
        return new ManData
        {
            Name = name ?? NameSelector.GetRandom(),
            role = role ?? RoleSelector.GetRandom(),
            trait = trait ?? TraitSelector.GetRandom(),
            StationsNeeded = stationsNeeded ?? StationsSelector.GetRandom()
        };
    }
}