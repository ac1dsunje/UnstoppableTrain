using TMPro;
using UnityEngine;

public class EndOverlayManager : ScreenManager
{
    [SerializeField] private TextMeshProUGUI _hintText;
    [SerializeField] private TextMeshProUGUI _endText;

    public override void ShowScreen()
    {
        _endText.text = "No drivers left in the train...";
        _hintText.text = "Press R to respawn";
        base.Show();
    }

    public override void HideScreen()
    {
        base.Hide();
    }
}