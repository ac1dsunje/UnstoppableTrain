
using System;
using UnityEngine;

[Serializable]
public class TraitConfig
{
    [Range(0f, 1f)]
    public float BaseChance = 0.10f;

    [Range(0f, 1f)]
    public float ScaleChance = 0.10f;
}