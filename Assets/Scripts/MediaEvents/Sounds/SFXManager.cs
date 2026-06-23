using UnityEngine;
using System.Collections;

public class SoundFXManager : ObjectPoolManager<AudioSource>
{
    private void OnEnable() => MediaEvents.OnSoundNeeded += HandleSoundNeeded;
    private void OnDisable() => MediaEvents.OnSoundNeeded -= HandleSoundNeeded;

    private void HandleSoundNeeded(SoundData sound, Vector3 pos) => PlaySound(sound, pos);

    public void PlaySound(SoundData soundData, Vector3 spawnPosition)
    {
        if (soundData == null || !soundData.HasSounds) return;

        AudioClip clip = soundData.Clips[Random.Range(0, soundData.Clips.Length)];
        float pitch = Random.Range(soundData.MinPitch, soundData.MaxPitch);
        float duration = clip.length / pitch;

        AudioSource audioSource = Get();
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
        Release(source);
    }

    protected override AudioSource Create()
    {
        GameObject go = new GameObject("PooledAudio");
        go.transform.SetParent(transform);
        return go.AddComponent<AudioSource>();
    }

    protected override void OnRelease(AudioSource item)
    {
        item.Stop();
        item.clip = null;
        item.pitch = 1f;
        item.volume = 1f;
        base.OnRelease(item);
    }
}