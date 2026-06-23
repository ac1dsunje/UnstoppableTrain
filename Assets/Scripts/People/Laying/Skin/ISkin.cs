using System;
using UnityEngine;

public interface ISkin
{
    public MeshRenderer GetShape();
    public event Action<ManData> OnManDataInitialized;
}