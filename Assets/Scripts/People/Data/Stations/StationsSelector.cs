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

public static class StationsSelector
{
    private static StationsRange _currentRange;

    public static int GetRandom()
    {
        return Random.Range(_currentRange.MinAmount, _currentRange.MaxAmount + 1);
    }

    public static void SetRange(StationsRange range)
    {
        _currentRange = range;
    }
}