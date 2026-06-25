using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnvironmentFactory
{
    private readonly PoolConfig _poolConfig;

    private readonly Dictionary<GameObject, ObjectPool<GameObject>> _poolsByPrefab = new();
    private readonly Dictionary<GameObject, ObjectPool<GameObject>> _instanceToPool = new();

    public EnvironmentFactory(PoolConfig poolConfig)
    {
        _poolConfig = poolConfig;
    }

    public GameObject Get(
        GameObject prefab,
        Vector3 position,
        Transform parent,
        Vector3 scale)
    {
        var pool = GetOrCreatePool(prefab);
        var env = pool.Get();

        env.transform.SetParent(parent, false);
        env.transform.position = position;
        env.transform.localScale = scale;

        _instanceToPool[env] = pool;

        return env;
    }

    public void Release(GameObject env)
    {
        if (_instanceToPool.TryGetValue(env, out var pool))
        {
            pool.Release(env);
            _instanceToPool.Remove(env);
        }
    }

    private ObjectPool<GameObject> GetOrCreatePool(GameObject prefab)
    {
        if (!_poolsByPrefab.ContainsKey(prefab))
        {
            _poolsByPrefab[prefab] = new ObjectPool<GameObject>(
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

    private GameObject Create(GameObject prefab)
    {
        return Object.Instantiate(prefab);
    }

    private void OnGet(GameObject item) => item.SetActive(true);
    private void OnRelease(GameObject item) => item.SetActive(false);
    private void OnDestroyItem(GameObject item) => Object.Destroy(item);
}