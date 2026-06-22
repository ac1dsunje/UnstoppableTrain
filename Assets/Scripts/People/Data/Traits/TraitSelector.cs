using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class TraitSelector
{
    // ToDo: use SetWeights in bootstrap
    private static List<TraitWeight> _weights = new(TraitPresets.Normal);

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