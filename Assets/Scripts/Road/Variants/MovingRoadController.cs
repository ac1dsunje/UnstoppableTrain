public class MovingRoadController : RoadController
{
    public override RoadType GetRoadType => RoadType.Moving;

    protected override void OnRoadActivated()
    {
        if (_gameStateManager.TryEnterEventState())
        {
            MediaEvents.TriggerEvent(transform.position, _onEnterSound);
        }
    }
}