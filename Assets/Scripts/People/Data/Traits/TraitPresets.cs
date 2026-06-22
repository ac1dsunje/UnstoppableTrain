public struct TraitWeight
{
    public Trait Trait;
    public float Weight;

    public TraitWeight(Trait trait, float weight)
    {
        Trait = trait;
        Weight = weight;
    }
}

//ToDo : use SO and send it to selector in bootstrap
public static class TraitPresets
{
    public static readonly TraitWeight[] Easy = new[]
    {
        new TraitWeight(Trait.Wallflower, 40f),
        new TraitWeight(Trait.Leader,     30f),
        new TraitWeight(Trait.Psychopath, 10f),
        new TraitWeight(Trait.Empath,     40f),
    };

    public static readonly TraitWeight[] Normal = new[]
    {
        new TraitWeight(Trait.Wallflower, 25f),
        new TraitWeight(Trait.Leader,     25f),
        new TraitWeight(Trait.Psychopath, 25f),
        new TraitWeight(Trait.Empath,     25f),
    };

    public static readonly TraitWeight[] Hardcore = new[]
    {
        new TraitWeight(Trait.Wallflower, 15f),
        new TraitWeight(Trait.Leader,     20f),
        new TraitWeight(Trait.Psychopath, 50f),
        new TraitWeight(Trait.Empath,     15f),
    };
}