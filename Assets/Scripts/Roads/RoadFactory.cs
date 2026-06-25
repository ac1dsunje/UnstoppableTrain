using UnityEngine;
using UnityEngine.Pool;

public class RoadFactory
{
    private readonly RailFactory _railFactory;
    private readonly EnvironmentFactory _environmentFactory;

    private readonly GameObject _roadPrefab;
    private readonly PoolConfig _poolConfig;

    private readonly ObjectPool<RoadController> _pool;

    public RoadFactory(
        RailFactory railFactory,
        EnvironmentFactory environmentFactory,
        GameObject roadPrefab,
        PoolConfig poolConfig)
    {
        _railFactory = railFactory;
        _environmentFactory = environmentFactory;
        _roadPrefab = roadPrefab;
        _poolConfig = poolConfig;

        _pool = new(
            createFunc: Create,
            actionOnGet: OnGet,
            actionOnRelease: OnRelease,
            actionOnDestroy: OnDestroyItem,
            collectionCheck: false,
            defaultCapacity: _poolConfig.DefaultCapacity,
            maxSize: _poolConfig.MaxSize
        );
    }

    public RoadController Get(RoadSegmentConfigSO segmentConfig, Vector3 position, Transform parent)
    {
        var road = _pool.Get();

        road.transform.SetParent(parent, false);
        road.transform.position = position;

        road.Initialize(_railFactory, _environmentFactory, segmentConfig);
        road.SetupData();

        return road;
    }

    public void Release(RoadController road)
    {
        _pool.Release(road);
    }

    private RoadController Create()
    {
        return Object.Instantiate(_roadPrefab).GetComponent<RoadController>();
    }

    private void OnGet(RoadController item) => item.gameObject.SetActive(true);
    private void OnRelease(RoadController item) => item.gameObject.SetActive(false);
    private void OnDestroyItem(RoadController item) => Object.Destroy(item.gameObject);
}