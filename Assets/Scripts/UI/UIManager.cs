using UnityEngine;

public class UIManager : MonoBehaviour 
{
    [SerializeField] private GameManager _gameManager;

    [SerializeField] private TrainController _train;

    private LayingMenOverlayManager _layingMenOverlay;
    private MainOverlayManager _mainOverlayManager;
    private SocialOverlyManager _socialOverlaymanager;

    private void OnEnable()
    {
        _gameManager.OnStateChanged += HandleStateChanged;
    }

    private void OnDisable()
    {
        _gameManager.OnStateChanged -= HandleStateChanged;
    }

    private void Awake()
    {
        _layingMenOverlay = GetComponent<LayingMenOverlayManager>().Initialize(_train);
        _mainOverlayManager = GetComponent<MainOverlayManager>().Initialize(_train);
        _socialOverlaymanager = GetComponent<SocialOverlyManager>().Initialize(_gameManager);
    }

    private void HandleStateChanged(GameState state)
    {
        switch (state)
        {
            case GameState.moving:
                _layingMenOverlay.HideScreen();
                _mainOverlayManager.ShowScreen();
                _socialOverlaymanager.HideScreen();
                break;

            case GameState.choosing:
                _layingMenOverlay.ShowScreen();
                _mainOverlayManager.HideScreen();
                _socialOverlaymanager.HideScreen();
                break;

            case GameState.social:
                _layingMenOverlay.HideScreen();
                _mainOverlayManager.HideScreen();
                _socialOverlaymanager.ShowScreen();
                break;
        }
}
}
