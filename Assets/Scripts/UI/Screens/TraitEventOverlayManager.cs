using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TraitEventOverlayManager : ScreenManager
{
    [SerializeField] private Button _acceptButton;
    [SerializeField] private TextMeshProUGUI _infoText;

    private GameManager _gameManager;
    private TraitManager _socialManager;

    public TraitEventOverlayManager Initialize(GameManager gm, TraitManager sm)
    {
        HideScreen();
        _gameManager = gm;

        _socialManager = sm;
        _socialManager.OnMessageGenerated += AddMessage;
        _socialManager.OnPhaseFinished += OnPhaseFinished;

        return this;
    }

    private void OnEnable() => _acceptButton.onClick.AddListener(AcceptButtonPress);

    private void OnDisable()
    {
        _acceptButton.onClick.RemoveListener(AcceptButtonPress);
        _socialManager.OnMessageGenerated -= AddMessage;
        _socialManager.OnPhaseFinished -= OnPhaseFinished;
    }

    private void AddMessage(string message)
    {
        _infoText.text += message + "\n";
    }

    private void OnPhaseFinished()
    {
        _acceptButton.gameObject.SetActive(true);
    }

    private void AcceptButtonPress()
    {
        _infoText.text = "";
        _acceptButton.gameObject.SetActive(false);
        _gameManager.SetMovingState();
    }

    public override void HideScreen() => Hide();

    public override void ShowScreen()
    {
        _infoText.text = "";
        _acceptButton.gameObject.SetActive(false);
        Show();
    }
}