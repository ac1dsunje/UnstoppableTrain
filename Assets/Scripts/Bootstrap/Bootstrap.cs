using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{

    private IEnumerator Start()
    {
        // ToDo: init modules here

        var loadingDuration = 1f;
        while (loadingDuration > 0f)
        {
            loadingDuration = Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadScene("GamePlay");
    }
}