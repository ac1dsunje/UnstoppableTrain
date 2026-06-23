using System.Collections;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [Scene]
    [SerializeField] private string GamePlay;
    [Scene]
    [SerializeField] private string LoadingScene;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private IEnumerator Start()
    {
        SceneLoader.Initialize(LoadingScene, GamePlay);

        yield return SceneLoader.SetGamePlaySceneAsync();
    }
}