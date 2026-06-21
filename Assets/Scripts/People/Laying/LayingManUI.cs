using UnityEngine;

public class LayingManUI : ScreenManager
{
    [SerializeField] private LayingManSlotUI _slot;
    [SerializeField] private LayingManController _controller;

    public void OnMouseEnter()
    {
        if (_controller.isActive)
        {
            _slot.Set(_controller.Data);
            ShowScreen();
        }
    }

    public void OnMouseExit()
    {
        if (_controller.isActive)
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