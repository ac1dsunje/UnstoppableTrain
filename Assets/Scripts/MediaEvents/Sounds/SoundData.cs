using UnityEngine;

[System.Serializable]
public class SoundData
{
    [field: SerializeField] public AudioClip[] Clips { get; private set; }
    [field: SerializeField] public float Volume { get; private set; } = 1f;
    [field: SerializeField] public float MinPitch { get; private set; } = 0.9f;
    [field: SerializeField] public float MaxPitch { get; private set; } = 1.1f;

    public bool HasSounds => Clips != null && Clips.Length > 0;
}