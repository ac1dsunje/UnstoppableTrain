using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public enum Role
{
    Driver,
    Mechanic,
    Doctor,
    NoSkill
}

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

public class RoleSelector
{
    private List<RoleWeight> _weights;

    public RoleSelector(IEnumerable<RoleWeight> weights)
    {
        _weights = weights.ToList();
    }

    public Role GetRandom()
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
}