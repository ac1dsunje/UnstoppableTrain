public class ManDataFactory
{
    private readonly NameSelector _nameSelector;
    private readonly RoleSelector _roleSelector;
    private readonly TraitSelector _traitSelector;
    private readonly StationsSelector _stationsSelector;

    public ManDataFactory(
        NameSelector nameSelector,
        RoleSelector roleSelector,
        TraitSelector traitSelector,
        StationsSelector stationsSelector)
    {
        _nameSelector = nameSelector;
        _roleSelector = roleSelector;
        _traitSelector = traitSelector;
        _stationsSelector = stationsSelector;
    }

    public ManData Create(
        string name = null,
        Role? role = null,
        Trait? trait = null,
        int? stationsNeeded = null)
    {
        return new ManData
        {
            Name = name ?? _nameSelector.GetRandom(),
            role = role ?? _roleSelector.GetRandom(),
            trait = trait ?? _traitSelector.GetRandom(),
            StationsNeeded = stationsNeeded ?? _stationsSelector.GetRandom()
        };
    }
}