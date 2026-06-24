using UnityEngine;

public class PassengerController : MonoBehaviour
{
    private TrainController _train;
    private ManData _data = new();
    private TraitFactory _traitFactory;

    public ManData GetData => _data;
    public ITraitBehaviour TraitBehavior { get; private set; }

    private void OnDisable()
    {
        _train.OnStationPassed -= CheckStationIndex;
    }

    public PassengerController Initialize(TrainController train, ManData data, TraitFactory traitFactory)
    {
        _train = train;
        _data = data;
        _data.StationsLeft = _data.StationsNeeded;
        _traitFactory = traitFactory;

        TraitBehavior = _traitFactory.Create(data.trait);

        _train.OnStationPassed += CheckStationIndex;
        return this;
    }

    public void CheckStationIndex()
    {
        _data.StationsLeft--;
    }

    public bool CheckLeave()
    {
        if (_data.StationsLeft <= 0)
        {
            return true;
        }
        return false;
    }

    public bool TryLeave()
    {
        if (_data.StationsLeft <= 0)
        {
            Leave();
            return true;
        }
        return false;
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