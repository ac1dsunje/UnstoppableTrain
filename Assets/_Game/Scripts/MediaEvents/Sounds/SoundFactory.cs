using UnityEngine;
using UnityEngine.Animations;

public class SoundFactory : PooledComponentFactory<AudioSource>
{
    private readonly GameObject _sfxPrefab;
    private readonly Transform _parent;

    public SoundFactory(GameObject sfxPrefab, PoolConfig poolConfig, Transform parent) : base(poolConfig)
    {
        _sfxPrefab = sfxPrefab;
        _parent = parent;
    }

    protected override AudioSource Create(GameObject prefab)
    {
        return Object.Instantiate(prefab, _parent).GetComponent<AudioSource>();
    }

    public AudioSource Get(Vector3 position)
    {
        var audioSource = GetItem(_sfxPrefab);
        audioSource.transform.position = position;
        return audioSource;
    }
}