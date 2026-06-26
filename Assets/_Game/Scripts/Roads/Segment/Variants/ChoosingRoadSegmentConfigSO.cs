using UnityEngine;

[CreateAssetMenu(fileName = "ChoosingRoad", menuName = "Game/Roads/Choosing Road")]
public class ChoosingRoadSegmentConfigSO : RoadSegmentConfigSO
{
    public override void OnSetup(RoadContext context)
    {
        int randLeft = Random.Range(1, MaxMenOnTheRail + 1);
        context.Road.LeftRail.SpawnManyLayingMen(randLeft);

        int randRight = Random.Range(1, MaxMenOnTheRail + 1);
        context.Road.RightRail.SpawnManyLayingMen(randRight);
    }

    public override void OnActivated(RoadContext context)
    {
        context.GameStateManager.EnterIn<ChoosingState>();
        context.MediaEventsBus.TriggerEvent(context.Road.transform.position, OnEnterSound);
    }

    public override void OnRailCleared(RoadContext context, RailController clearedRail, RailController remainingRail)
    {
        foreach (var passenger in remainingRail.LayingMen)
        {
            context.Train.TryTakeNewPassenger(passenger.Data);
        }
        remainingRail.ClearLayingMen();
    }
}