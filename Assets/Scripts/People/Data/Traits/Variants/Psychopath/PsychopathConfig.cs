using System;
using UnityEngine;

[Serializable]
public class PsychopathConfig
{
    [Range(0f, 1f)]
    public float BaseChance = 0.10f;

    [Range(0f, 1f)]
    public float ScaleChance = 0.10f;
}