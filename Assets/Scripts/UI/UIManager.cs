using UnityEngine;

public class UIManager : MonoBehaviour 
{
    [SerializeField] private GameManager _gameManager;

    [SerializeField] private TrainController _train;
    [SerializeField] private TraitManager _socialManager;

    private MainOverlayManager _mainOverlayManager;
    private TraitEventOverlayManager _socialOverlaymanager;

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
        _mainOverlayManager = GetComponent<MainOverlayManager>().Initialize(_train);
        _socialOverlaymanager = GetComponent<TraitEventOverlayManager>().Initialize(_gameManager, _socialManager);
    }

    private void HandleStateChanged(GameState state)
    {
        switch (state)
        {
            case GameState.moving:
                _mainOverlayManager.ShowScreen();
                _socialOverlaymanager.HideScreen();
                break;

            case GameState.choosing:
                _mainOverlayManager.ShowScreen();
                _socialOverlaymanager.HideScreen();
                break;

            case GameState.social:
                _mainOverlayManager.HideScreen();
                _socialOverlaymanager.ShowScreen();
                break;
        }
    }
}