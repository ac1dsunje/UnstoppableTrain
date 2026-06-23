using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    [SerializeField] private SocialEventManager _socialManager;
    [SerializeField] private EpidemicEventManager _epidemicManager;
    [SerializeField] private BreakdownEventManager _breakdownManager;
    [SerializeField] private StationManager _stationManager;

    public event Action<string> OnMessageGenerated;
    public event Action OnPhaseFinished;

    private TrainController _train;

    public GameEventsManager Initialize(TrainController train)
    {
        _train = train;

        SubscribeOnEventManager(_socialManager);
        SubscribeOnEventManager(_epidemicManager);
        SubscribeOnEventManager(_breakdownManager);
        SubscribeOnEventManager(_stationManager);

        return this;
    }

    private void SubscribeOnEventManager(PhaseManagerBase manager)
    {
        manager.OnMessageGenerated += msg => OnMessageGenerated?.Invoke(msg);
        manager.OnPhaseFinished += () => OnPhaseFinished?.Invoke();
    }

    public bool TryStartEvent()
    {
        int roll = UnityEngine.Random.Range(0, 6);

        List<PassengerController> passengers = new();

        if (roll < 3)
            passengers = new List<PassengerController>(_train.GetPassengers());

        switch (roll)
        {
            case 0: return _socialManager.TryStartSocialPhase(passengers);
            case 1: _epidemicManager.StartEpidemicPhase(passengers); return true;
            case 2: _breakdownManager.StartBreakdownPhase(passengers); return true;
            default: return false;
        }
    }

    public bool TryEnterStationEvent(List<PassengerController> passengers)
    {
        return _stationManager.TryStartStationPhase(passengers);
    }
}