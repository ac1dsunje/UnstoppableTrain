public class EndState : IGameState
{
    private readonly TrainController _train;

    public EndState(TrainController train)
    {
        _train = train;
    }

    public void Enter()
    {
        _train.SetSpeedScale(0f);
    }

    public void Exit() { } // из EndState не выходим
}