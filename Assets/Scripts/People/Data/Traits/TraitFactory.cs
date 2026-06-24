public enum Trait
{
    Wallflower,
    Leader,
    Psychopath,
    Empath
}

public class TraitFactory
{
    private TraitsConfigSO _config;

    public TraitFactory(TraitsConfigSO config)
    {
        _config = config;
    }

    public ITrait Create(Trait trait)
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