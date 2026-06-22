using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

[Serializable]
public struct TraitWeight
{
    public Trait Trait;
    public float Weight;

    public TraitWeight(Trait trait, float weight)
    {
        Trait = trait;
        Weight = weight;
    }
}

public static class TraitSelector
{
    private static List<TraitWeight> _weights = new();

    public static Trait GetRandom()
    {
        float total = _weights.Sum(w => w.Weight);

        float roll = Random.Range(0f, total);

        foreach (var tw in _weights)
        {
            roll -= tw.Weight;
            if (roll <= 0f)
                return tw.Trait;
        }

        return _weights[_weights.Count - 1].Trait;
    }

    public static void SetWeights(IEnumerable<TraitWeight> newWeights)
    {
        _weights = newWeights.ToList();
    }

    public static void SetWeight(Trait trait, float weight)
    {
        for (int i = 0; i < _weights.Count; i++)
        {
            if (_weights[i].Trait == trait)
            {
                _weights[i] = new TraitWeight(trait, weight);
                return;
            }
        }
        _weights.Add(new TraitWeight(trait, weight));
    }
}