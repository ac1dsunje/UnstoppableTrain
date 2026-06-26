using UnityEngine;

public abstract class RoadSegmentConfigSO : ScriptableObject
{
    [field: SerializeField] public float RoadLength { get; private set; }
    [field: SerializeField] public float Weight { get; private set; }
    [field: SerializeField] public SoundData OnEnterSound { get; private set; }
    [field: SerializeField] public EnvironmentAtlasSO EnvironmentAtlas { get; private set; }
    [field: SerializeField] public int MaxMenOnTheRail { get; private set; }
    [field: SerializeField] public GameObject RailPrefab { get; private set; }

    [field: SerializeField] public float RailXOffset { get; private set; } = 1.5f;
    [field: SerializeField] public float EnvironmentXMultiplier { get; private set; } = 3f;

    public virtual bool IsStation => false;
    public abstract void OnSetup(RoadContext context);
    public abstract void OnActivated(RoadContext context);
    public abstract void OnRailCleared(RoadContext context, RailController clearedRail, RailController remainingRail);
}