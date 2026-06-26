using System;
using Random = UnityEngine.Random;

[Serializable]
public class StationsRange
{
    public int MinAmount;
    public int MaxAmount;

    public StationsRange(int min, int max)
    {
        MinAmount = min;
        MaxAmount = max;
    }
}

public class StationsSelector
{
    private StationsRange _currentRange;

    public StationsSelector(StationsRange range)
    {
        _currentRange = range;
    }

    public int GetRandom()
    {
        return Random.Range(_currentRange.MinAmount, _currentRange.MaxAmount + 1);
    }
}