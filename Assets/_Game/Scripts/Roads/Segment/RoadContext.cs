public class RoadContext
{
    public RoadController Road { get; private set; }
    public TrainController Train { get; private set; }
    public GameStateManager GameStateManager { get; private set; }
    public MediaEventsBus MediaEventsBus { get; private set; }

    public RoadContext(RoadController road, TrainController train, GameStateManager gameStateManager, MediaEventsBus mediaEventsBus)
    {
        Road = road;
        Train = train;
        GameStateManager = gameStateManager;
        MediaEventsBus = mediaEventsBus;
    }
}