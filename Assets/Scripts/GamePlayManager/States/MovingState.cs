public class MovingState : IGameState
{
    private readonly TrainController _train;
    private readonly CameraController _cam;

    public MovingState(TrainController train, CameraController cam)
    {
        _train = train;
        _cam = cam;
    }

    public void Enter()
    {
        _train.SetSpeedScale(1f);
        _cam.SetMovingPos();
    }

    public void Exit() { }
}