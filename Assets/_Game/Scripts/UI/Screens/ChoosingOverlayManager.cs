public class ChoosingOverlayManager : ScreenManager
{
    public override void ShowScreen()
    {
        _screenCanvasGroup.alpha = 1.0f;
    }

    public override void HideScreen()
    {
        _screenCanvasGroup.alpha = 0f;
    }
}