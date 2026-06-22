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

public static class StationsPresets
{
    public static readonly StationsRange Easy = new(15, 30);

    public static readonly StationsRange Normal = new(7, 20);

    public static readonly StationsRange Hardcore = new(1, 15);
}