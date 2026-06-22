using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    [Scene]
    [SerializeField] private string GamePlay;

    [SerializeField] private RolePreset rolePreset;
    [SerializeField] private TraitPreset traitPreset;
    [SerializeField] private StationsPreset stationtPreset;

    private IEnumerator Start()
    {
        RoleSelector.SetWeights(rolePreset.Weights);
        TraitSelector.SetWeights(traitPreset.Weights);
        StationsSelector.SetRange(stationtPreset.Range);

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(GamePlay);
    }
}