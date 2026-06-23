using UnityEngine;

public class LayingManUI : ScreenManager
{
    [SerializeField] private LayingManSlotUI _slot;
    [SerializeField] private LayingManController _controller;

    private void OnMouseEnter()
    {
        if (_controller.IsActive)
        {
            _slot.Set(_controller.Data);
            ShowScreen();
        }
    }

    private void OnMouseExit()
    {
        if (_controller.IsActive)
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