using UnityEngine;
using UnityEngine.Pool;

public class LayingManFactory
{
    private readonly ManGeneralConfigSO _manConfig;
    private readonly GameObject _layingManPrefab;
    private readonly ManDataFactory _manDataFactory;

    private readonly PoolConfig _poolConfig;

    private readonly ObjectPool<LayingManController> _pool;

    public LayingManFactory(
        ManGeneralConfigSO manConfig, 
        GameObject layingManPrefab,
        ManDataFactory manDataFactory,
        PoolConfig poolConfig)
    {
        _manConfig = manConfig;
        _layingManPrefab = layingManPrefab;
        _manDataFactory = manDataFactory;
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

    public LayingManController Get(Vector3 position, Transform parent)
    {
        var man = _pool.Get();

        man.transform.SetParent(parent, false);
        man.transform.position = position; 
        
        man.SetupData();

        return man;
    }

    public void Release(LayingManController man)
    {
        _pool.Release(man);
    }

    private LayingManController Create()
    {
        var man = Object.Instantiate(
            _layingManPrefab
        ).GetComponent<LayingManController>();

        man.Initialize(_manConfig, _manDataFactory);
        man.SetupData();

        return man;
    }

    private void OnGet(LayingManController item) => item.gameObject.SetActive(true);

    private void OnRelease(LayingManController item) => item.gameObject.SetActive(false);

    private void OnDestroyItem(LayingManController item) => Object.Destroy(item.gameObject);
}