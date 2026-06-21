using UnityEngine;

public class PassengerController : MonoBehaviour
{
    [SerializeField] private TrainController _train;
    [SerializeField] private ManData _data = new();
    
    public ManData GetData => _data;
    public ITrait TraitBehavior { get; private set; }

    private void OnDisable()
    {
        if (_train != null)
            _train.OnStationPassed -= CheckStationIndex;
    }

    public PassengerController Initialize(TrainController train, ManData data)
    {
        _train = train;
        _data = data;
        _data.StationsLeft = _data.StationsNeeded;

        TraitBehavior = TraitFactory.Create(data.trait);

        _train.OnStationPassed += CheckStationIndex;

        return this;
    }

    private void CheckStationIndex()
    {
        _data.StationsLeft--;
        if (_data.StationsLeft <= 0)
        {
            Leave();
        }
    }

    private void Leave()
    {
        _train.OnStationPassed -= CheckStationIndex;
        _train.GetPassengerOut(this);
        Destroy(gameObject);
    }

    public void Kill()
    {
        _train.OnStationPassed -= CheckStationIndex;
        _train.GetPassengerOut(this);
        Destroy(gameObject);
    }
}