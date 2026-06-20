using TMPro;
using UnityEngine;

public class MainOverlayManager : ScreenManager
{
    [SerializeField] private TextMeshProUGUI _chunksPassedText;
    [SerializeField] private TrainController _train;

    private void OnEnable()
    {
        _train.OnStatsUpdated += UpdateStats;
    }

    private void OnDisable()
    {
        _train.OnStatsUpdated -= UpdateStats;
    }

    public void ShowScreen()
    {
        base.Show();
    }

    public void HideScreen()
    {
        base.Hide();
    }

    private void UpdateStats(TrainStats stats)
    {
        _chunksPassedText.text = $"chunks passed: {stats.chunksPassed}";
    }
}