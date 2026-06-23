using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEventsManager
{
    private readonly MonoBehaviour _coroutineRunner;
    private readonly float _messageDelay;

    public event Action<string> OnMessageGenerated;
    public event Action OnPhaseFinished;

    private TrainController _train;
    private SocialEventManager _socialManager;
    private EpidemicEventManager _epidemicManager;
    private BreakdownEventManager _breakdownManager;
    private StationManager _stationManager;

    public GameEventsManager(MonoBehaviour coroutineRunner, TrainController train, float messageDelay = 1.2f)
    {
        _coroutineRunner = coroutineRunner;
        _train = train;
        _messageDelay = messageDelay;

        InitializeManagers();
    }

    private void InitializeManagers()
    {
        _socialManager = new SocialEventManager(_coroutineRunner, _messageDelay);
        _epidemicManager = new EpidemicEventManager(_coroutineRunner, _messageDelay);
        _breakdownManager = new BreakdownEventManager(_coroutineRunner, _messageDelay);
        _stationManager = new StationManager(_coroutineRunner, _messageDelay);

        SubscribeOnEventManager(_socialManager);
        SubscribeOnEventManager(_epidemicManager);
        SubscribeOnEventManager(_breakdownManager);
        SubscribeOnEventManager(_stationManager);
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