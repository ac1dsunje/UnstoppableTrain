using System;
using UnityEngine;

public class RoadController : MonoBehaviour
{
    public Action<GameObject, bool> OnRoadStateChanged;

    [SerializeField] private float roadLength = 10f; // Длина дороги по оси Z
    public float RoadLength => roadLength;

    private bool _isLeftActive = false;
    private bool _isRightActive = false;
    private bool _isRoadActive = false;

    [SerializeField] private RailController _leftRail;
    [SerializeField] private RailController _rightRail;

    private void OnEnable()
    {
        _leftRail.OnThisActive += OnLeftRailStateChanged;
        _rightRail.OnThisActive += OnRightRailStateChanged;
    }

    private void OnDisable()
    {
        _leftRail.OnThisActive -= OnLeftRailStateChanged;
        _rightRail.OnThisActive -= OnRightRailStateChanged;
    }

    private void OnLeftRailStateChanged(bool state)
    {
        _isLeftActive = state;
        UpdateRoadState();
    }

    private void OnRightRailStateChanged(bool state)
    {
        _isRightActive = state;
        UpdateRoadState();
    }

    private void UpdateRoadState()
    {
        bool shouldBeActive = _isLeftActive || _isRightActive;

        if (_isRoadActive != shouldBeActive)
        {
            _isRoadActive = shouldBeActive;
            OnRoadStateChanged?.Invoke(gameObject, _isRoadActive);
        }
    }
}