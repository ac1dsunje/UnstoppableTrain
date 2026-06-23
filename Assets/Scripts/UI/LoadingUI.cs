using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour
{
    [SerializeField] private Image _loadingProgressImage;

    private static LoadingUI _instance;

    private void Awake()
    {
        _instance = this;
    }

    private void OnDestroy()
    {
        _instance = null;
    }

    public static void SetProgress(float progress)
    {
        if (_instance == null || _instance._loadingProgressImage == null) return;

        progress = Mathf.Clamp01(progress);
        _instance._loadingProgressImage.fillAmount = progress;
    }
}