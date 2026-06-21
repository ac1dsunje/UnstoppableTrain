using System;
using System.Linq;
using TMPro;
using UnityEngine;

public class MainOverlayManager : ScreenManager
{
    [SerializeField] private TextMeshProUGUI _chunksPassedText;
    
    [SerializeField] private TextMeshProUGUI _rolesText;
    [SerializeField] private TextMeshProUGUI _traitsText;

    private TrainController _train;

    private void OnEnable()
    {
        _train.OnStatsUpdated += UpdateStats;
    }

    private void OnDisable()
    {
        _train.OnStatsUpdated -= UpdateStats;
    }

    public MainOverlayManager Initialize(TrainController train)
    {
        _train = train;
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
        _chunksPassedText.text = $"chunks passed: {stats.chunksPassed}";

        _rolesText.text = GetRolesInfo(stats);
        _traitsText.text = GetTraitsInfo(stats);
    }

    private string GetRolesInfo(TrainStats stats)
    {
        if (stats._passengers == null || stats._passengers.Count == 0)
            return " ";

        string info = "";
        foreach (Role role in Enum.GetValues(typeof(Role)))
        {
            int count = stats._passengers.Count(p => p.role == role);
            info += $"{role}: {count}\n";
        }

        return info.TrimEnd('\n');
    }

    private string GetTraitsInfo(TrainStats stats)
    {
        if (stats._passengers == null || stats._passengers.Count == 0)
            return " ";

        string info = "";
        foreach (Trait trait in Enum.GetValues(typeof(Trait)))
        {
            int count = stats._passengers.Count(p => p.trait == trait);
            info += $"{trait}: {count}\n";
        }

        return info.TrimEnd('\n');
    }
}