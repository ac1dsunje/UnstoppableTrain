using UnityEngine;
using Random = UnityEngine.Random;

public static class StationsSelector
{
    private static StationsRange _currentRange = StationsPresets.Normal;

    public static int GetRandom()
    {
        return Random.Range(_currentRange.MinAmount, _currentRange.MaxAmount + 1);
    }

    public static void SetRange(StationsRange newRange)
    {
        _currentRange = newRange;
    }

    public static void SetRange(int min, int max)
    {
        _currentRange = new StationsRange(min, max);
    }
}