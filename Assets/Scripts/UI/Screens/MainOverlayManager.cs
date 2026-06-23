using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainOverlayManager : ScreenManager
{
    [SerializeField] private TextMeshProUGUI _chunksPassedText;
    [SerializeField] private Transform _passengersInfoContainer;
    [SerializeField] private PassengerInfoSlotUI _passengerInfoSlotPrefab;

    private TrainController _train;

    private List<PassengerInfoSlotUI> _passengersSlots = new();
    private List<PassengerController> _passengersControllers = new();

    private void OnDisable()
    {
        if (_train == null) return;
        _train.OnStatsUpdated -= UpdateStats;
    }

    public MainOverlayManager Initialize(TrainController train)
    {
        _train = train;
        _train.OnStatsUpdated += UpdateStats;
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

        DeletePassengers(passengers);
        AddOrRefreshPassengers(passengers);
    }

    private void DeletePassengers(List<PassengerController> passengers)
    {
        for (int i = _passengersControllers.Count - 1; i >= 0; i--)
        {
            var controller = _passengersControllers[i];
            if (controller == null || !passengers.Contains(controller))
            {
                if (_passengersSlots[i] != null)
                    Destroy(_passengersSlots[i].gameObject);

                _passengersSlots.RemoveAt(i);
                _passengersControllers.RemoveAt(i);
            }
        }
    }

    private void AddOrRefreshPassengers(List<PassengerController> passengers)
    {
        foreach (var passenger in passengers)
        {
            if (passenger == null) continue;

            int index = _passengersControllers.IndexOf(passenger);
            if (index == -1)
                SpawnPassenger(passenger);
            else
                _passengersSlots[index].Refresh();
        }
    }

    private void SpawnPassenger(PassengerController passenger)
    {
        var item = Instantiate(_passengerInfoSlotPrefab, _passengersInfoContainer);
        var slotUI = item.GetComponent<PassengerInfoSlotUI>().Initialize(passenger.GetData);
        _passengersControllers.Add(passenger);
        _passengersSlots.Add(slotUI);
    }
}