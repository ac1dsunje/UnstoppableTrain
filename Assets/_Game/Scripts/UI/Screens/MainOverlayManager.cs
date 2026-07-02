using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainOverlayManager : ScreenManager
{
    [SerializeField] private TextMeshProUGUI _chunksPassedText;
    [SerializeField] private Transform _passengersInfoContainer;

    private TrainController _train;
    private PassengerInfoSlotUIFactory _passengerInfoSlotUIFactory;

    private readonly Dictionary<PassengerController, PassengerInfoSlotUI> _passengerSlots = new();

    private readonly List<PassengerController> _keysToRemove = new();

    private void OnDisable()
    {
        _train.OnStatsUpdated -= UpdateStats;
    }

    public MainOverlayManager Initialize(TrainController train, PassengerInfoSlotUIFactory passengerInfoSlotUIFactory)
    {
        _train = train;
        _train.OnStatsUpdated += UpdateStats;

        _passengerInfoSlotUIFactory = passengerInfoSlotUIFactory;

        UpdateStats(_train.GetStats());
        return this;
    }

    public override void ShowScreen() => Show();
    public override void HideScreen() => Hide();

    private void UpdateStats(TrainStats stats)
    {
        _chunksPassedText.text = $"Stations passed: {stats.stationsPassed}";
        UpdatePassengers(stats.Passengers);
    }

    private void UpdatePassengers(List<PassengerController> passengers)
    {
        if (passengers == null) return;

        RemoveObsoleteSlots(passengers);
        AddOrRefreshSlots(passengers);
    }

    private void RemoveObsoleteSlots(List<PassengerController> actualPassengers)
    {
        _keysToRemove.Clear();

        foreach (var pair in _passengerSlots)
        {
            if (pair.Key == null || !actualPassengers.Contains(pair.Key))
            {
                _keysToRemove.Add(pair.Key);
                _passengerInfoSlotUIFactory.Release(pair.Value);
            }
        }

        foreach (var key in _keysToRemove)
        {
            _passengerSlots.Remove(key);
        }
    }

    private void AddOrRefreshSlots(List<PassengerController> passengers)
    {
        foreach (var passenger in passengers)
        {
            if (passenger == null) continue;

            if (_passengerSlots.TryGetValue(passenger, out var slot))
            {
                slot.Refresh();
            }
            else
            {
                CreatePassengerSlot(passenger);
            }
        }
    }

    private void CreatePassengerSlot(PassengerController passenger)
    {
        var slot = _passengerInfoSlotUIFactory.Get(_passengersInfoContainer, passenger.GetData);

        _passengerSlots.Add(passenger, slot);
    }
}