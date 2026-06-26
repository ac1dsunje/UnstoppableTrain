using UnityEngine;

public class RoadFactory : PooledComponentFactory<RoadController>
{
    private readonly RailFactory _railFactory;
    private readonly EnvironmentFactory _environmentFactory;
    private readonly GameObject _roadPrefab;

    public RoadFactory(
        RailFactory railFactory,
        EnvironmentFactory environmentFactory,
        GameObject roadPrefab,
        PoolConfig poolConfig)
        : base(poolConfig)
    {
        _railFactory = railFactory;
        _environmentFactory = environmentFactory;
        _roadPrefab = roadPrefab;
    }

    protected override RoadController Create(GameObject prefab)
    {
        RoadController road = Object.Instantiate(prefab).GetComponent<RoadController>();
        road.SetContainers();
        return road;
    }

    public RoadController Get(
        RoadSegmentConfigSO segmentConfig,
        Vector3 position,
        Transform parent,
        TrainController train,
        GameStateManager gameStateManager)
    {
        var road = GetItem(_roadPrefab);

        road.transform.SetParent(parent, false);
        road.transform.position = position;

        road.Initialize(_railFactory, _environmentFactory, segmentConfig, train, gameStateManager);
        road.SetupData();

        return road;
    }
}