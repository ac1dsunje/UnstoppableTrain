using UnityEngine;

public class EnvironmentFactory : PooledGameObjectFactory
{
    public EnvironmentFactory(PoolConfig poolConfig) : base(poolConfig) { }

    protected override GameObject Create(GameObject prefab)
    {
        return Object.Instantiate(prefab);
    }

    public GameObject Get(
        GameObject prefab,
        Vector3 position,
        Transform parent,
        Vector3 scale)
    {
        var env = GetItem(prefab);
        env.transform.SetParent(parent, false);
        env.transform.position = position;
        env.transform.localScale = scale;
        return env;
    }
}