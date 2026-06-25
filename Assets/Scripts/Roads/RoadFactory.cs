using UnityEngine;
using UnityEngine.Pool;

public class RoadFactory
{
    private readonly RailFactory _railFactory;
    private readonly RoadsConfigSO _roadConfig;

    // todo: add to constructor
    private readonly int _defaultCapacity = 10;
    private readonly int _maxSize = 10;

    private readonly ObjectPool<RoadController> _pool;

    public RoadFactory(RailFactory railFactory, RoadsConfigSO roadConfig)
    {
        _railFactory = railFactory;
        _roadConfig = roadConfig;

        _pool = new(
            createFunc: Create,
            actionOnGet: OnGet,
            actionOnRelease: OnRelease,
            actionOnDestroy: OnDestroyItem,
            collectionCheck: false,
            defaultCapacity: _defaultCapacity,
            maxSize: _maxSize
        );
    }

    public RoadController Get(RoadSegmentConfigSO segmentConfig, Vector3 position, Transform parent)
    {
        var road = _pool.Get();

        road.transform.SetParent(parent, false);
        road.transform.position = position;

        road.Initialize(_railFactory, segmentConfig);

        return road;
    }

    public void Release(RoadController road)
    {
        _pool.Release(road);
    }

    private RoadController Create()
    {
        RoadController road = Object.Instantiate(
            _roadConfig.RoadPrefab
        ).GetComponent<RoadController>();
        return road;
    }

    private void OnGet(RoadController item) => item.gameObject.SetActive(true);

    private void OnRelease(RoadController item) => item.gameObject.SetActive(false);

    private void OnDestroyItem(RoadController item) => Object.Destroy(item.gameObject);
}