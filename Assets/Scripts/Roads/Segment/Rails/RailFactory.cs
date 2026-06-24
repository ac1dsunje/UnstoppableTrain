using UnityEngine;
using Random = UnityEngine.Random;

public class RailFactory
{
    private readonly LayingManFactory _layingManFactory;

    public RailFactory(LayingManFactory layingManFactory)
    {
        _layingManFactory = layingManFactory;
    }

    public RailController Create(RoadSegmentConfigSO config, Vector3 position, Transform parent, float xOffset, bool xFlip)
    {
        RailController rail = Object.Instantiate(
            config.RailPrefab,
            position,
            Quaternion.identity,
            parent
        ).GetComponent<RailController>();

        int rand = Random.Range(0, config.EnvironmentAtlas.EnvironmentObjects.Count);
        Transform railTransform = rail.transform;

        Transform envTransform = Object.Instantiate(
            config.EnvironmentAtlas.EnvironmentObjects[rand],
            new Vector3(railTransform.position.x + 2 * xOffset, railTransform.position.y, railTransform.position.z),
            Quaternion.identity,
            railTransform
        ).transform;

        if (xFlip) envTransform.localScale = new Vector3(-1, 1, 1);

        rail.Initialize(_layingManFactory);
        return rail;
    }
}