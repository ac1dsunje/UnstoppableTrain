using UnityEngine;

public class LayingManFactory : PooledComponentFactory<LayingManController>
{
    private readonly ManGeneralConfigSO _manConfig;
    private readonly GameObject _layingManPrefab;
    private readonly ManDataFactory _manDataFactory;

    public LayingManFactory(
        ManGeneralConfigSO manConfig,
        GameObject layingManPrefab,
        ManDataFactory manDataFactory,
        PoolConfig poolConfig) : base(poolConfig)
    {
        _manConfig = manConfig;
        _layingManPrefab = layingManPrefab;
        _manDataFactory = manDataFactory;
    }

    protected override LayingManController Create(GameObject prefab)
    {
        var man = Object.Instantiate(prefab).GetComponent<LayingManController>();
        man.Initialize(_manConfig, _manDataFactory);
        return man;
    }

    public LayingManController Get(Vector3 position, Transform parent)
    {
        var man = GetItem(_layingManPrefab);
        man.transform.SetParent(parent, false);
        man.transform.position = position;
        man.SetupData();
        return man;
    }
}