using System;
using UnityEngine;

public class MediaEventsBus
{
    public event Action<SoundData, Vector3> OnSoundNeeded;

    public void TriggerEvent(Vector3 position, SoundData sound = null)
    {
        if (sound != null && sound.HasSounds)
            OnSoundNeeded?.Invoke(sound, position);
    }
}