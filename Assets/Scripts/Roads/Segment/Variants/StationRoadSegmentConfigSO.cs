using UnityEngine;

[CreateAssetMenu(fileName = "StationRoad", menuName = "Game/Roads/Station Road")]
public class StationRoadSegmentConfigSO : RoadSegmentConfigSO
{
    public override bool IsStation => true;

    public override void OnSetup(RoadContext context) { }

    public override void OnActivated(RoadContext context)
    {
        if (context.GameStateManager.TryEnterStationEvent())
        {
            MediaEvents.TriggerEvent(context.Road.transform.position, OnEnterSound);
        }
    }

    public override void OnRailCleared(RoadContext context, RailController clearedRail, RailController remainingRail) { }
}