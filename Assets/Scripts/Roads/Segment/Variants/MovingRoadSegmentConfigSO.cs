using UnityEngine;

[CreateAssetMenu(fileName = "MovingRoad", menuName = "Game/Roads/Moving Road")]
public class MovingRoadSegmentConfigSO : RoadSegmentConfigSO
{
    public override void OnSetup(RoadContext context) { }

    public override void OnActivated(RoadContext context)
    {
        if (context.GameStateManager.TryEnterEventState())
        {
            context.MediaEventsBus.TriggerEvent(context.Road.transform.position, OnEnterSound);
        }
    }

    public override void OnRailCleared(RoadContext context, RailController clearedRail, RailController remainingRail) { }
}