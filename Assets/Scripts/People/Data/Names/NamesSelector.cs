using UnityEngine;

public class NameSelector
{
    private readonly string[] _maleNames = new string[]
    {
        "Alex", "Jordan", "Casey", "Riley", "Quinn", "Morgan",
        "Cameron", "Dakota", "Finley", "Jamie", "Jesse",
        "Logan", "Parker", "Reese", "Robin", "Rowan",
        "Sawyer", "Drew", "Ellis", "Hayden", "Tatum"
    };

    private readonly string[] _femaleNames = new string[]
    {
        "Taylor", "Avery", "Emerson", "Harper", "Kendall",
        "Peyton", "Sage", "Sydney", "Lennox", "Casey",
        "Riley", "Quinn", "Morgan", "Dakota", "Finley",
        "Jamie", "Robin", "Rowan", "Sydney", "Hayden"
    };

    public string GetRandom()
    {
        string[] selectedNames = Random.value < 0.5f ? _maleNames : _femaleNames;
        return selectedNames[Random.Range(0, selectedNames.Length)];
    }
}