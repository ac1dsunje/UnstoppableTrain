public class MovingRoadController : RoadController
{
    public override RoadType GetRoadType => RoadType.Moving;

    protected override void OnRoadActivated()
    {
        _gameStateManager.TryEnterEventState();
    }
}