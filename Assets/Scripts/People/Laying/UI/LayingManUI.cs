using UnityEngine;

public class LayingManUI : ScreenManager
{
    [SerializeField] private LayingManSlotUI _slot;

    private LayingManController _controller;

    private void Awake()
    {
        _controller = GetComponentInParent<LayingManController>();
        HideScreen();
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

    public override void HideScreen() => Hide();
    public override void ShowScreen() => Show();
}