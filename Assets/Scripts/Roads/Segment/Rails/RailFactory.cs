using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class RailFactory
{
    private readonly LayingManFactory _layingManFactory;

    private readonly PoolConfig _poolConfig;

    private readonly Dictionary<GameObject, ObjectPool<RailController>> _poolsByPrefab = new();
    private readonly Dictionary<RailController, ObjectPool<RailController>> _railToPool = new();

    public RailFactory(LayingManFactory layingManFactory, PoolConfig poolConfig)
    {
        _layingManFactory = layingManFactory;
        _poolConfig = poolConfig;
    }

    public RailController Get(
        RoadSegmentConfigSO config,
        Vector3 position,
        Transform parent,
        float xOffset,
        bool xFlip)
    {
        var pool = GetOrCreatePool(config.RailPrefab);
        var rail = pool.Get();

        rail.transform.SetParent(parent, false);
        rail.transform.position = position;

        rail.Initialize(_layingManFactory);

        _railToPool[rail] = pool;

        return rail;
    }

    public void Release(RailController rail)
    {
        if (_railToPool.TryGetValue(rail, out var pool))
        {
            pool.Release(rail);
            _railToPool.Remove(rail);
        }
    }

    private ObjectPool<RailController> GetOrCreatePool(GameObject prefab)
    {
        if (!_poolsByPrefab.ContainsKey(prefab))
        {
            _poolsByPrefab[prefab] = new ObjectPool<RailController>(
                createFunc: () => Create(prefab),
                actionOnGet: OnGet,
                actionOnRelease: OnRelease,
                actionOnDestroy: OnDestroyItem,
                collectionCheck: false,
                defaultCapacity: _poolConfig.DefaultCapacity,
                maxSize: _poolConfig.MaxSize
            );
        }
        return _poolsByPrefab[prefab];
    }

    private RailController Create(GameObject prefab)
    {
        return Object.Instantiate(prefab).GetComponent<RailController>();
    }

    private void OnGet(RailController item) => item.gameObject.SetActive(true);
    private void OnRelease(RailController item) => item.gameObject.SetActive(false);
    private void OnDestroyItem(RailController item) => Object.Destroy(item.gameObject);
}