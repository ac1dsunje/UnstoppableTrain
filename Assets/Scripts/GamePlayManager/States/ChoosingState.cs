public class ChoosingState : IGameState
{
    private readonly TrainController _train;
    private readonly CameraController _cam;

    public ChoosingState(TrainController train, CameraController cam)
    {
        _train = train;
        _cam = cam;
    }

    public void Enter()
    {
        _train.SetSpeedScale(0f);
        _cam.SetChoosingPos();
    }

    public void Exit() { }
}