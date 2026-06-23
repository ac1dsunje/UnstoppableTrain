using System.Collections;

public class StationState : IGameState
{
    private readonly TrainController _train;
    private readonly CameraController _cam;
    private readonly GameStateManager _manager;
    private readonly CoroutineRunner _runner;

    public StationState(TrainController train, CameraController cam,
                        GameStateManager manager, CoroutineRunner runner)
    {
        _train = train;
        _cam = cam;
        _manager = manager;
        _runner = runner;
    }

    public void Enter()
    {
        _train.SetSpeedScale(0f);
        _cam.SetChoosingPos();
        _runner.Run(WaitAtStation());
    }

    public void Exit() { }

    private IEnumerator WaitAtStation()
    {
        // ToDo: add passengers getting out animation
        yield return new UnityEngine.WaitForSeconds(2f);
        _manager.EnterIn<MovingState>();
    }
}