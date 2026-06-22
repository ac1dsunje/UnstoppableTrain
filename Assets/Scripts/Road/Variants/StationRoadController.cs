public class StationRoadController : RoadController
{
    public override RoadType GetRoadType => RoadType.Station;

    protected override void OnRoadActivated()
    {
        _gameManager.SetStationState();
    }
}