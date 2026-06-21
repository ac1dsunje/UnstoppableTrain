using UnityEngine;

public class PassengerController : MonoBehaviour
{
    [SerializeField] private TrainController _train;
    [SerializeField] private ManData _data = new();
    public ManData GetData => _data;

    private void OnDisable()
    {
        _train.OnStatsUpdated -= CheckStationIndex;
    }

    public PassengerController Initialize(TrainController train, ManData data)
    {
        _train = train;
        _data = data;
        _data.StationsLeft = _data.StationsNeeded;

        _train.OnStatsUpdated += CheckStationIndex;

        return this;
    }

    private void CheckStationIndex(TrainStats stats)
    {
        _data.StationsLeft--;
        if (_data.StationsLeft == 0)
        {
            Leave();
        }
    }

    private void Leave()
    {
        _train.OnStatsUpdated -= CheckStationIndex;
        _train.GetPassengerOut(this);
        Destroy(gameObject);
    }
}