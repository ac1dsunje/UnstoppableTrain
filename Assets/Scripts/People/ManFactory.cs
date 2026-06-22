using System;
using UnityEngine;
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

    private const int DefaultMinStations = 1;
    private const int DefaultMaxStations = 15;

    public static ManData Create(
        string name = null,
        Role? role = null,
        Trait? trait = null,
        int? stationsNeeded = null,
        int? minStations = null,
        int? maxStations = null)
    {
        if (string.IsNullOrEmpty(name))
        {
            name = _neutralNames[Random.Range(0, _neutralNames.Length)];
        }

        Role actualRole = role ?? (Role)Random.Range(0, Enum.GetValues(typeof(Role)).Length);

        Trait actualTrait = trait ?? (Trait)Random.Range(0, Enum.GetValues(typeof(Trait)).Length);

        int actualStations;
        if (stationsNeeded.HasValue)
        {
            actualStations = stationsNeeded.Value;
        }
        else
        {
            int min = minStations ?? DefaultMinStations;
            int max = maxStations ?? DefaultMaxStations;
            actualStations = Random.Range(min, max + 1);
        }

        return new ManData
        {
            Name = name,
            role = actualRole,
            trait = actualTrait,
            StationsNeeded = actualStations
        };
    }
}