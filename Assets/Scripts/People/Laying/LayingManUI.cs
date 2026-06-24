using UnityEngine;

public class LayingManUI : ScreenManager
{
    [SerializeField] private LayingManSlotUI _slot;

    private LayingManController _controller;
    public void Initialize(LayingManController controller)
    {
        _controller = controller;
    }

    private void OnMouseEnter()
    {
        if (_controller != null && _controller.IsActive)
        {
            _slot.Set(_controller.Data);
            ShowScreen();
        }
    }

    private void OnMouseExit()
    {
        if (_controller != null && _controller.IsActive)
        {
            HideScreen();
        }
    }

    private void Awake()
    {
        HideScreen();
    }

    public override void HideScreen()
    {
        Hide();
    }

    public override void ShowScreen()
    {
        Show();
    }
}