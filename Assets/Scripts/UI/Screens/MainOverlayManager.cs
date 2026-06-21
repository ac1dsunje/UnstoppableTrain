using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class MainOverlayManager : ScreenManager
{
    [SerializeField] private TextMeshProUGUI _chunksPassedText;
    
    [SerializeField] private TextMeshProUGUI _rolesText;
    [SerializeField] private TextMeshProUGUI _traitsText;

    private TrainController _train;

    private void OnDisable()
    {
        _train.OnStatsUpdated -= UpdateStats;
    }

    public MainOverlayManager Initialize(TrainController train)
    {
        _train = train;
        _train.OnStatsUpdated += UpdateStats;
        return this;
    }

    public override void ShowScreen()
    {
        Show();
    }

    public override void HideScreen()
    {
        Hide();
    }

    private void UpdateStats(TrainStats stats)
    {
        _chunksPassedText.text = $"Stations passed: {stats.chunksPassed}";

        _rolesText.text = GetRolesInfo(_train.Passengers);
        _traitsText.text = GetTraitsInfo(_train.Passengers);
    }

    // todo: add passenger info slot ui & delete code under

    private string GetRolesInfo(List<PassengerController> passengers)
    {
        if (passengers == null || passengers.Count == 0)
            return " ";

        string info = "";
        foreach (Role role in Enum.GetValues(typeof(Role)))
        {
            int count = passengers.Count(p => p.Data.role == role);
            info += $"{role}: {count}\n";
        }

        return info.TrimEnd('\n');
    }

    private string GetTraitsInfo(List<PassengerController> passengers)
    {
        if (passengers == null || passengers.Count == 0)
            return " ";

        string info = "";
        foreach (Trait trait in Enum.GetValues(typeof(Trait)))
        {
            int count = passengers.Count(p => p.Data.trait == trait);
            info += $"{trait}: {count}\n";
        }

        return info.TrimEnd('\n');
    }
}