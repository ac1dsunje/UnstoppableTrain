using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    [Scene]
    [SerializeField] private string GamePlay;
    [Scene]
    [SerializeField] private string LoadingScene;

    private IEnumerator Start()
    {

        SceneManager.LoadScene(LoadingScene, LoadSceneMode.Additive);

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(GamePlay);
    }
}