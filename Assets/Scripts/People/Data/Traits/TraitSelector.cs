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

public class TraitSelector
{
    private List<TraitWeight> _weights;

    public TraitSelector(IEnumerable<TraitWeight> weights)
    {
        _weights = weights.ToList();
    }

    public Trait GetRandom()
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
}