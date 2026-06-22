using System;
using UnityEngine;
using System.Collections.Generic;

public class GameEventsManager : MonoBehaviour
{
    [SerializeField] private TrainController _train;
    [SerializeField] private SocialEventManager _socialManager;
    [SerializeField] private EpidemicEventManager _epidemicManager;
    [SerializeField] private BreakdownEventManager _breakdownManager;

    public event Action<string> OnMessageGenerated;
    public event Action OnPhaseFinished;

    private void Awake()
    {
        _socialManager.OnMessageGenerated += msg => OnMessageGenerated?.Invoke(msg);
        _socialManager.OnPhaseFinished += () => OnPhaseFinished?.Invoke();

        _epidemicManager.OnMessageGenerated += msg => OnMessageGenerated?.Invoke(msg);
        _epidemicManager.OnPhaseFinished += () => OnPhaseFinished?.Invoke();

        _breakdownManager.OnMessageGenerated += msg => OnMessageGenerated?.Invoke(msg);
        _breakdownManager.OnPhaseFinished += () => OnPhaseFinished?.Invoke();
    }

    public bool TryStartEvent()
    {
        int roll = UnityEngine.Random.Range(0, 3);

        List<PassengerController> passengers = new();

        if (roll < 3)
        {
            passengers = new List<PassengerController>(_train.GetPassengers());
        }

        switch (roll)
        {
            case 0: return TryStartSocial(passengers);
            case 1: TryStartEpidemic(passengers); return true;
            case 2: TryStartBreakdown(passengers); return true;
            default: return false;
        }
    }

    private bool TryStartSocial(List<PassengerController> passengers)
    {
        return _socialManager.TryStartSocialPhase(passengers);
    }

    private void TryStartEpidemic(List<PassengerController> passengers)
    {
        _epidemicManager.StartEpidemicPhase(passengers);
    }

    private void TryStartBreakdown(List<PassengerController> passengers)
    {
        _breakdownManager.StartBreakdownPhase(passengers);
    }
}