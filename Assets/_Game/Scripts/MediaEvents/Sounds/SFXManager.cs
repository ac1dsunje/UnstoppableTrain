using System;
using System.Collections;
using UnityEngine;

public class SFXManager : IDisposable
{
    private readonly SoundFactory _soundFactory;
    private readonly CoroutineRunner _coroutineRunner;
    private readonly MediaEventsBus _mediaEventsBus;

    public SFXManager(SoundFactory soundFactory, CoroutineRunner coroutineRunner, MediaEventsBus mediaEvents)
    {
        _soundFactory = soundFactory;
        _coroutineRunner = coroutineRunner;
        _mediaEventsBus = mediaEvents;

        _mediaEventsBus.OnSoundNeeded += HandleSoundNeeded;
    }

    private void HandleSoundNeeded(SoundData sound, Vector3 pos) => PlaySound(sound, pos);

    public void PlaySound(SoundData soundData, Vector3 spawnPosition)
    {
        if (soundData == null || !soundData.HasSounds) return;

        AudioClip clip = soundData.Clips[UnityEngine.Random.Range(0, soundData.Clips.Length)];
        float pitch = UnityEngine.Random.Range(soundData.MinPitch, soundData.MaxPitch);
        float duration = clip.length / pitch;

        AudioSource audioSource = _soundFactory.Get(spawnPosition);
        audioSource.clip = clip;
        audioSource.volume = Mathf.Clamp01(soundData.Volume);
        audioSource.pitch = pitch;
        audioSource.Play();

        _coroutineRunner.StartCoroutine(ReturnAfterDelay(audioSource, duration));
    }

    private IEnumerator ReturnAfterDelay(AudioSource source, float delay)
    {
        yield return new WaitForSeconds(delay);
        _soundFactory.Release(source);
    }

    public void Dispose()
    {
        _mediaEventsBus.OnSoundNeeded -= HandleSoundNeeded;
    }
}