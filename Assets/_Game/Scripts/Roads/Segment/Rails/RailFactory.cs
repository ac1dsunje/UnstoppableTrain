using UnityEngine;

public class RailFactory : PooledComponentFactory<RailController>
{
    private readonly LayingManFactory _layingManFactory;

    public RailFactory(LayingManFactory layingManFactory, PoolConfig poolConfig)
        : base(poolConfig)
    {
        _layingManFactory = layingManFactory;
    }

    protected override RailController Create(GameObject prefab)
    {
        return Object.Instantiate(prefab).GetComponent<RailController>();
    }

    public RailController Get(
        RoadSegmentConfigSO config,
        Vector3 position,
        Transform parent)
    {
        var rail = GetItem(config.RailPrefab);
        rail.transform.SetParent(parent, false);
        rail.transform.position = position;
        rail.Initialize(_layingManFactory);
        return rail;
    }
}