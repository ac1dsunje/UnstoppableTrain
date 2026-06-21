using UnityEngine;

public abstract class ScreenManager : MonoBehaviour
{
    [SerializeField] protected CanvasGroup _screenCanvasGroup;

    protected void Show()
    {
        _screenCanvasGroup.alpha = 1;
        _screenCanvasGroup.blocksRaycasts = true;
        _screenCanvasGroup.interactable = true;
    }

    protected void Hide()
    {
        _screenCanvasGroup.alpha = 0;
        _screenCanvasGroup.blocksRaycasts = false;
        _screenCanvasGroup.interactable = false;
    }

    public abstract void ShowScreen();
    public abstract void HideScreen();
}