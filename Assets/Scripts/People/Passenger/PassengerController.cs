using UnityEngine;

public class PassengerController : MonoBehaviour
{
    [SerializeField] private TrainController _train;
    public ManData Data { get; private set; } = new();
    

    private void OnDisable()
    {
        _train.OnStatsUpdated -= CheckStationIndex;
    }

    public PassengerController Initialize(TrainController train, ManData data)
    {
        _train = train;
        Data = data;
        Data.StationsLeft = Data.StationsNeeded;

        _train.OnStatsUpdated += CheckStationIndex;

        return this;
    }

    private void CheckStationIndex(TrainStats stats)
    {
        Data.StationsLeft--;
        if (Data.StationsLeft == 0)
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