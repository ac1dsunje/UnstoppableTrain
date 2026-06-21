using UnityEngine;

public class PassengerController : MonoBehaviour
{
    [SerializeField] private TrainController _train;
    [SerializeField] private int _stationsLeft;
    [SerializeField] private ManData _data = new();

    private void OnDisable()
    {
        _train.OnStatsUpdated -= CheckStationIndex;
    }

    public PassengerController Initialize(TrainController train, ManData data)
    {
        _train = train;
        _data = data;
        _stationsLeft = _data.StationsNeeded;

        _train.OnStatsUpdated += CheckStationIndex;

        return this;
    }

    private void CheckStationIndex(TrainStats stats)
    {
        _stationsLeft--;
        if (_stationsLeft == 0)
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