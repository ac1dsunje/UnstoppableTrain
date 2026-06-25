using UnityEngine;
using UnityEngine.Pool;

public class LayingManFactory
{
    private readonly ManGeneralConfigSO _manConfig;
    private readonly ManVisualConfigSO _manVisualConfig;
    private readonly ManDataFactory _manDataFactory;

    // todo: add to constructor
    private readonly int _defaultCapacity = 6;
    private readonly int _maxSize = 10;

    private readonly ObjectPool<LayingManController> _pool;

    public LayingManFactory(
        ManGeneralConfigSO manConfig, 
        ManVisualConfigSO manVisualConfig,
        ManDataFactory manDataFactory)
    {
        _manConfig = manConfig;
        _manVisualConfig = manVisualConfig;
        _manDataFactory = manDataFactory;

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

    public LayingManController Get(Vector3 position, Transform parent)
    {
        var man = _pool.Get();

        man.transform.SetParent(parent, false);
        man.transform.position = position;

        return man;
    }

    public void Release(LayingManController man)
    {
        _pool.Release(man);
    }

    private LayingManController Create()
    {
        var man = Object.Instantiate(
            _manVisualConfig.LayingManPrefab
        ).GetComponent<LayingManController>();

        man.Initialize(_manConfig, _manDataFactory);

        return man;
    }

    private void OnGet(LayingManController item) => item.gameObject.SetActive(true);

    private void OnRelease(LayingManController item) => item.gameObject.SetActive(false);

    private void OnDestroyItem(LayingManController item) => Object.Destroy(item.gameObject);
}