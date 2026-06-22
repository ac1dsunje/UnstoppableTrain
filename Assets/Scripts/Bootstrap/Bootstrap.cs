using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    [Scene]
    [SerializeField] private string GamePlay;

    private IEnumerator Start()
    {
        // ToDo: init modules here

        yield return new WaitForSeconds(1f); // just imitation of loading

        SceneManager.LoadScene(GamePlay);
    }
}