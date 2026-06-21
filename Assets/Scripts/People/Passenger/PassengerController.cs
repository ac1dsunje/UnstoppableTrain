using UnityEngine;

public class PassengerController : MonoBehaviour
{
    [SerializeField] private TrainController _train;
    private ManData _data = new();
    private int _startStation;

    private void OnDisable()
    {
        _train.OnStatsUpdated -= CheckStationIndex;
    }

    public PassengerController Initialize(TrainController train, ManData data, int currentStation)
    {
        _train = train;
        _data = data;
        _startStation = currentStation;

        _train.OnStatsUpdated += CheckStationIndex;

        return this;
    }

    private void CheckStationIndex(TrainStats stats)
    {
        if(stats.chunksPassed -  _startStation >= _data.chunks)
        {
            Leave();
        }
    }

    private void Leave()
    {
        _train.OnStatsUpdated -= CheckStationIndex;
        _train.GetPassengerOut(_data);
        Destroy(gameObject);
    }
}