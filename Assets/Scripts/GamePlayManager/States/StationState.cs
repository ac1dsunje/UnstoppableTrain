using System.Collections.Generic;

public class StationState : IGameState
{
    private readonly TrainController _train;
    private readonly CameraController _cam;
    private readonly StationManager _stationManager;

    public StationState(TrainController train, CameraController cam, StationManager stationManager)
    {
        _train = train;
        _cam = cam;
        _stationManager = stationManager;
    }

    public void Enter()
    {
        _train.Stop();
        _cam.SetBirdView();

        List<PassengerController> passengers = new(_train.GetPassengers());
        _stationManager.StartStationPhase(passengers);
    }

    public void Exit()
    {
    }
}