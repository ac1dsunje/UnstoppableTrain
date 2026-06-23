using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    private static string _loadingScene;
    private static string _gamePlayScene;

    public static void Initialize(string loadingScene, string gamePlayScene)
    {
        _loadingScene = loadingScene;
        _gamePlayScene = gamePlayScene;
    }

    public static IEnumerator SetGamePlaySceneAsync()
    {
        return LoadSceneAsync(_gamePlayScene);
    }

    public static IEnumerator RestartGameAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(_gamePlayScene);
        while (!operation.isDone)
        {
            yield return null;
        }
    }

    private static IEnumerator LoadSceneAsync(string targetScene)
    {
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(_loadingScene);
        while (!loadingOperation.isDone)
        {
            yield return null;
        }

        LoadingUI.SetProgress(0f);

        AsyncOperation targetOperation = SceneManager.LoadSceneAsync(targetScene, LoadSceneMode.Additive);
        targetOperation.allowSceneActivation = false;

        float currentProgress = 0f;
        while (targetOperation.progress < 0.9f)
        {
            float targetProgress = targetOperation.progress / 0.9f;
            currentProgress = Mathf.MoveTowards(currentProgress, targetProgress, Time.deltaTime * 0.5f);
            LoadingUI.SetProgress(currentProgress);
            yield return null;
        }

        while (currentProgress < 1f)
        {
            currentProgress = Mathf.MoveTowards(currentProgress, 1f, Time.deltaTime * 1.5f);
            LoadingUI.SetProgress(currentProgress);
            yield return null;
        }

        targetOperation.allowSceneActivation = true;

        while (!targetOperation.isDone)
        {
            yield return null;
        }

        yield return SceneManager.UnloadSceneAsync(_loadingScene);
    }
}