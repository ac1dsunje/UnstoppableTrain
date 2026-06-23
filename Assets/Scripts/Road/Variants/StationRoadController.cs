public class StationRoadController : RoadController
{
    public override RoadType GetRoadType => RoadType.Station;

    protected override void OnRoadActivated()
    {
        if(_gameStateManager.TryEnterStationEvent())
        {
            MediaEvents.TriggerEvent(transform.position, _onEnterSound);
        }
    }
}