using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SocialOverlyManager : ScreenManager
{
    [SerializeField] private Button _acceptButton;
    [SerializeField] private TextMeshProUGUI _infoText;
    private GameManager _gameManager;

    private void OnEnable()
    {
        _acceptButton.onClick.AddListener(AcceptButtonPress);
    }

    private void OnDisable()
    {
        _acceptButton.onClick.RemoveListener(AcceptButtonPress);
    }

    private void AcceptButtonPress()
    {
        _gameManager.SetMovingState();
    }

    public SocialOverlyManager Initialize(GameManager _gm)
    {
        _gameManager = _gm;
        return this;
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