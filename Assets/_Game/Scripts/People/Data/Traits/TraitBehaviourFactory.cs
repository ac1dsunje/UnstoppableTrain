public enum Trait
{
    Wallflower,
    Leader,
    Psychopath,
    Empath
}

public class TraitBehaviourFactory
{
    private TraitsConfigSO _config;

    public TraitBehaviourFactory(TraitsConfigSO config)
    {
        _config = config;
    }

    public ITraitBehaviour Create(Trait trait)
    {
        switch (trait)
        {
            case Trait.Empath:
                return new EmpathTraitBehaviour(_config.EmpathConfig);
            case Trait.Leader:
                return new LeaderTraitBehaviour(_config.LeaderConfig);
            case Trait.Psychopath:
                return new PsychopathTraitBehaviour(_config.PsychopathConfig);
            case Trait.Wallflower:
                return new WallflowerTraitBehaviour(_config.WallflowerConfig);
            default:
                return new WallflowerTraitBehaviour(_config.WallflowerConfig);
        }
    }
}