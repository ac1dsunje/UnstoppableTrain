using Random = UnityEngine.Random;

public static class StationsSelector
{
    private static StationsRange _currentRange = StationsPresets.Normal;

    public static int GetRandom()
    {
        return Random.Range(_currentRange.MinAmount, _currentRange.MaxAmount + 1);
    }
}