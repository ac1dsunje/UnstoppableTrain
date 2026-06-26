using System.Collections.Generic;
using Random = UnityEngine.Random;

public class RoadSelector
{
    private List<RoadSegmentConfigSO> _configs = new();
    
    public RoadSelector(IEnumerable<RoadSegmentConfigSO> configs)
    {
        _configs = new List<RoadSegmentConfigSO>(configs);
    }

    public RoadSegmentConfigSO GetRandom()
    {

        float totalWeight = 0f;
        foreach (var config in _configs)
        {
            totalWeight += config.Weight;
        }

        float roll = Random.Range(0f, totalWeight);

        foreach (var config in _configs)
        {
            roll -= config.Weight;
            if (roll <= 0f)
                return config;
        }

        return _configs[_configs.Count - 1];
    }
}