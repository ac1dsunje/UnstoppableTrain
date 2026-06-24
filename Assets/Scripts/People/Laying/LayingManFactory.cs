using UnityEngine;

public class LayingManFactory
{
    private readonly ManGeneralConfigSO _manConfig;
    private readonly ManVisualConfigSO _manVisualConfig;

    public LayingManFactory(ManGeneralConfigSO manConfig, ManVisualConfigSO manVisualConfig)
    {
        _manConfig = manConfig;
        _manVisualConfig = manVisualConfig;
    }

    public LayingManController Create(Vector3 position, Transform parent)
    {
        LayingManController man = Object.Instantiate(
            _manVisualConfig.LayingManPrefab,
            position,
            Quaternion.identity,
            parent
        ).GetComponent<LayingManController>();

        man.Initialize(_manConfig);
        return man;
    }
}