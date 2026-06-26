public class EndState : IGameState
{
    private readonly TrainController _train;

    public EndState(TrainController train)
    {
        _train = train;
    }

    public void Enter()
    {
        _train.Resume();
    }

    public void Exit() { }
}