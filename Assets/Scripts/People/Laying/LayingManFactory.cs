using UnityEngine;

public class LayingManFactory
{
    private readonly ManGeneralConfigSO _manConfig;
    private readonly ManVisualConfigSO _manVisualConfig;
    private readonly ManDataFactory _manDataFactory;

    public LayingManFactory(
        ManGeneralConfigSO manConfig, 
        ManVisualConfigSO manVisualConfig,
        ManDataFactory manDataFactory)
    {
        _manConfig = manConfig;
        _manVisualConfig = manVisualConfig;
        _manDataFactory = manDataFactory;
    }

    public LayingManController Create(Vector3 position, Transform parent)
    {
        LayingManController man = Object.Instantiate(
            _manVisualConfig.LayingManPrefab,
            position,
            Quaternion.identity,
            parent
        ).GetComponent<LayingManController>();

        man.Initialize(_manConfig, _manDataFactory);
        return man;
    }
}