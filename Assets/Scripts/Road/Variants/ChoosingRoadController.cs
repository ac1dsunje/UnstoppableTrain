using UnityEngine;

public class ChoosingRoadController : RoadController
{
    public override RoadType GetRoadType => RoadType.Choosing;

    protected override void InitializeRoad()
    {
        SpawnLayingMen();
    }

    private void SpawnLayingMen()
    {
        int randLeft = Random.Range(1, _maxMenOnTheRail + 1);
        LeftRail.SpawnManyLayingMen(randLeft);

        int randRight = Random.Range(1, _maxMenOnTheRail + 1);
        RightRail.SpawnManyLayingMen(randRight);
    }

    protected override void OnRoadActivated()
    {
        _gameStateManager.EnterIn<ChoosingState>();

        MediaEvents.TriggerEvent(transform.position, _onEnterSound);
    }

    protected override void OnRailCleared(RailController clearedRail, RailController remainingRail)
    {
        foreach (var passenger in remainingRail.LayingMen)
        {
            _train.TryTakeNewPassenger(passenger.Data);
            Destroy(passenger.gameObject);
        }
    }
}