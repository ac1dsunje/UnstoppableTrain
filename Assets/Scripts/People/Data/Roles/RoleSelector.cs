using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;


[Serializable]
public struct RoleWeight
{
    public Role Role;
    public float Weight;

    public RoleWeight(Role role, float weight)
    {
        Role = role;
        Weight = weight;
    }
}

public static class RoleSelector
{
    private static List<RoleWeight> _weights = new();

    public static Role GetRandom()
    {
        float total = _weights.Sum(w => w.Weight);

        float roll = Random.Range(0f, total);

        foreach (var tw in _weights)
        {
            roll -= tw.Weight;
            if (roll <= 0f)
                return tw.Role;
        }

        return _weights[_weights.Count - 1].Role;
    }

    public static void SetWeights(IEnumerable<RoleWeight> newWeights)
    {
        _weights = newWeights.ToList();
    }

    public static void SetWeight(Role role, float weight)
    {
        for (int i = 0; i < _weights.Count; i++)
        {
            if (_weights[i].Role == role)
            {
                _weights[i] = new RoleWeight(role, weight);
                return;
            }
        }
        _weights.Add(new RoleWeight(role, weight));
    }
}