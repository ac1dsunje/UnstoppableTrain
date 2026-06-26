using System.Collections.Generic;
using Random = UnityEngine.Random;

public class RoadSelector
{
    private List<RoadSegmentConfigSO> _configs = new();
    private readonly float _totalWeight;
    
    public RoadSelector(IEnumerable<RoadSegmentConfigSO> configs)
    {
        _configs = new List<RoadSegmentConfigSO>(configs);

        foreach (var config in _configs)
        {
            _totalWeight += config.Weight;
        }
    }

    public RoadSegmentConfigSO GetRandom()
    {
        float roll = Random.Range(0f, _totalWeight);

        foreach (var config in _configs)
        {
            roll -= config.Weight;
            if (roll <= 0f)
                return config;
        }

        return _configs[_configs.Count - 1];
    }
}