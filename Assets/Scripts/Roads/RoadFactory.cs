using UnityEngine;

public class RoadFactory
{
    private readonly RailFactory _railFactory;

    public RoadFactory(RailFactory railFactory)
    {
        _railFactory = railFactory;
    }

    public RoadController Create(RoadsConfigSO roadsConfig, RoadSegmentConfigSO segmentConfig, Vector3 position, Transform parent)
    {
        RoadController road = Object.Instantiate(
            roadsConfig.RoadPrefab,
            position,
            Quaternion.identity,
            parent
        ).GetComponent<RoadController>();

        road.Initialize(_railFactory, segmentConfig);
        return road;
    }
}