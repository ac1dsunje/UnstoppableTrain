public enum Trait
{
    Wallflower,
    Leader,
    Psychopath,
    Empath
}

public static class TraitFactory
{
    private static TraitsConfigSO _config;

    public static void SetConfig(TraitsConfigSO config)
    {
        _config = config;
    }

    public static ITrait Create(Trait trait)
    {
        switch (trait)
        {
            case Trait.Empath:
                return new EmpathTrait(_config.EmpathConfig);
            case Trait.Leader:
                return new LeaderTrait(_config.LeaderConfig);
            case Trait.Psychopath:
                return new PsychopathTrait(_config.PsychopathConfig);
            case Trait.Wallflower:
                return new WallflowerTrait(_config.WallflowerConfig);
            default:
                return new WallflowerTrait(_config.WallflowerConfig);
        }
    }
}