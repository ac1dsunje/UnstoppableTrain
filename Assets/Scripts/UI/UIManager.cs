using UnityEngine;

public class UIManager : MonoBehaviour 
{
    [SerializeField] private GameManager _gameManager;
    private LayingMenOverlayManager _layingMenOverlay;
    private MainOverlayManager _mainOverlayManager;

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
        _layingMenOverlay = GetComponent<LayingMenOverlayManager>();
        _mainOverlayManager = GetComponent<MainOverlayManager>();
    }

    private void HandleStateChanged(GameState state)
    {
        switch (state)
        {
            case GameState.moving:
                _layingMenOverlay.HideScreen();
                _mainOverlayManager.ShowScreen();
                break;

            case GameState.choosing:
                _layingMenOverlay.ShowScreen();
                _mainOverlayManager.HideScreen();
                break;
        }
}
}
