using UnityEngine;

public class GamePlayEntryPoint: MonoBehaviour
{
    [SerializeField] private RolePreset rolePreset;
    [SerializeField] private TraitPreset traitPreset;
    [SerializeField] private StationsPreset stationtPreset;

    private void Start()
    {
        InitializeManFactories();
    }   

    private void InitializeManFactories()
    {
        RoleSelector.SetWeights(rolePreset.Weights);
        TraitSelector.SetWeights(traitPreset.Weights);
        StationsSelector.SetRange(stationtPreset.Range);
    }
}
