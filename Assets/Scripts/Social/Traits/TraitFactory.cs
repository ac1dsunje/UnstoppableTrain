public static class TraitFactory
{
    public static ITrait Create(Trait trait)
    {
        switch (trait)
        {
            case Trait.Psychopath: return new PsychopathTrait();
            case Trait.Leader: return new LeaderTrait();
            case Trait.Empath: return new EmpathTrait();
            case Trait.Wallflower: return new WallflowerTrait();
            default: return new WallflowerTrait();
        }
    }
}