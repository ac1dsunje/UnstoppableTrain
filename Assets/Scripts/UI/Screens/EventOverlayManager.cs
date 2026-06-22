using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventOverlayManager : ScreenManager
{
    [SerializeField] private Button _acceptButton;
    [SerializeField] private TextMeshProUGUI _infoText;

    private GameManager _gameManager;
    private GameEventsManager _eventsManager;

    public EventOverlayManager Initialize(GameManager gm, GameEventsManager em)
    {
        HideScreen();
        _gameManager = gm;
        _eventsManager = em;

        _eventsManager.OnMessageGenerated += AddMessage;
        _eventsManager.OnPhaseFinished += OnPhaseFinished;

        return this;
    }

    private void OnEnable() => _acceptButton.onClick.AddListener(AcceptButtonPress);

    private void OnDisable()
    {
        _acceptButton.onClick.RemoveListener(AcceptButtonPress);

        if (_eventsManager == null) return;
        _eventsManager.OnMessageGenerated -= AddMessage;
        _eventsManager.OnPhaseFinished -= OnPhaseFinished;
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