using System;
using UnityEngine;

public static class MediaEvents
{
    public static event Action<SoundData, Vector3> OnSoundNeeded;

    public static void TriggerEvent(Vector3 position, SoundData sound = null)
    {
        if (sound != null && sound.HasSounds)
            OnSoundNeeded?.Invoke(sound, position);
    }
}