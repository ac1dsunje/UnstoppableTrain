using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    [Scene]
    [SerializeField] private string GamePlay;

    [SerializeField] private RolePreset rolePreset;
    [SerializeField] private TraitPreset traitPreset;

    private IEnumerator Start()
    {
        RoleSelector.SetWeights(rolePreset.Weights);
        TraitSelector.SetWeights(traitPreset.Weights);

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(GamePlay);
    }
}