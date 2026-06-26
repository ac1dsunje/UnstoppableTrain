using UnityEngine;

public class SkinHatFactory: PooledGameObjectFactory
{
    public SkinHatFactory(PoolConfig poolConfig) : base(poolConfig)
    {

    }

    protected override GameObject Create(GameObject prefab)
    {
        return Object.Instantiate(prefab);
    }

    public GameObject Get(
        GameObject prefab,
        Transform parent)
    {
        var env = GetItem(prefab);
        env.transform.SetParent(parent, false);
        return env;
    }
}