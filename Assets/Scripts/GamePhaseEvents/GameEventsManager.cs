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

        _socialManager.OnMessageGenerated += msg => OnMessageGenerated?.Invoke(msg);
        _socialManager.OnPhaseFinished += () => OnPhaseFinished?.Invoke();

        _epidemicManager.OnMessageGenerated += msg => OnMessageGenerated?.Invoke(msg);
        _epidemicManager.OnPhaseFinished += () => OnPhaseFinished?.Invoke();

        _breakdownManager.OnMessageGenerated += msg => OnMessageGenerated?.Invoke(msg);
        _breakdownManager.OnPhaseFinished += () => OnPhaseFinished?.Invoke();

        _stationManager.OnMessageGenerated += msg => OnMessageGenerated?.Invoke(msg);
        _stationManager.OnPhaseFinished += () => OnPhaseFinished?.Invoke();

        return this;
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