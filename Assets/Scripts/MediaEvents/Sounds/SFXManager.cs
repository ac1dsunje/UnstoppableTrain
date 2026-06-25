using UnityEngine;
using UnityEngine.Pool;
using System.Collections;

public class SoundFXManager : MonoBehaviour
{
    [SerializeField] private PoolConfig _soundPoolConfig;

    private ObjectPool<AudioSource> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<AudioSource>(
            createFunc: Create,
            actionOnGet: OnGet,
            actionOnRelease: OnRelease,
            actionOnDestroy: OnDestroyItem,
            collectionCheck: false,
            defaultCapacity: _soundPoolConfig.DefaultCapacity,
            maxSize: _soundPoolConfig.MaxSize
        );
    }

    private void OnEnable() => MediaEvents.OnSoundNeeded += HandleSoundNeeded;
    private void OnDisable() => MediaEvents.OnSoundNeeded -= HandleSoundNeeded;

    private void HandleSoundNeeded(SoundData sound, Vector3 pos) => PlaySound(sound, pos);

    public void PlaySound(SoundData soundData, Vector3 spawnPosition)
    {
        if (soundData == null || !soundData.HasSounds) return;

        AudioClip clip = soundData.Clips[Random.Range(0, soundData.Clips.Length)];
        float pitch = Random.Range(soundData.MinPitch, soundData.MaxPitch);
        float duration = clip.length / pitch;

        AudioSource audioSource = _pool.Get();
        audioSource.transform.position = spawnPosition;
        audioSource.clip = clip;
        audioSource.volume = Mathf.Clamp01(soundData.Volume);
        audioSource.pitch = pitch;
        audioSource.Play();

        StartCoroutine(ReturnAfterDelay(audioSource, duration));
    }

    private IEnumerator ReturnAfterDelay(AudioSource source, float delay)
    {
        yield return new WaitForSeconds(delay);
        _pool.Release(source);
    }

    private AudioSource Create()
    {
        GameObject go = new GameObject("PooledAudio");
        go.transform.SetParent(transform);
        return go.AddComponent<AudioSource>();
    }

    private void OnGet(AudioSource item) => item.gameObject.SetActive(true);

    private void OnRelease(AudioSource item)
    {
        item.Stop();
        item.clip = null;
        item.pitch = 1f;
        item.volume = 1f;
        item.gameObject.SetActive(false);
    }

    private void OnDestroyItem(AudioSource item) => Destroy(item.gameObject);
}