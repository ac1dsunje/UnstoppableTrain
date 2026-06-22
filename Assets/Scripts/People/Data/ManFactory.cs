using Random = UnityEngine.Random;

public static class ManFactory
{
    private static readonly string[] _neutralNames = new string[]
    {
        "Alex", "Taylor", "Jordan", "Casey", "Riley", "Avery", "Quinn", "Morgan",
        "Cameron", "Dakota", "Emerson", "Finley", "Harper", "Jamie", "Jesse",
        "Kendall", "Logan", "Parker", "Peyton", "Reese", "Robin", "Rowan",
        "Sage", "Sawyer", "Sydney", "Drew", "Ellis", "Hayden", "Lennox", "Tatum"
    };

    public static ManData Create(
    string name = null,
    Role? role = null,
    Trait? trait = null,
    int? stationsNeeded = null)
    {
        if (string.IsNullOrEmpty(name))
        {
            name = _neutralNames[Random.Range(0, _neutralNames.Length)];
        }

        Role actualRole = role ?? RoleSelector.GetRandom();
        Trait actualTrait = trait ?? TraitSelector.GetRandom();
        int actualStations = stationsNeeded ?? StationsSelector.GetRandom();

        return new ManData
        {
            Name = name,
            role = actualRole,
            trait = actualTrait,
            StationsNeeded = actualStations
        };
    }
}