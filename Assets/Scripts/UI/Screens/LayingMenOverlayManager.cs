using UnityEngine;
using System.Collections.Generic;

public class LayingMenOverlayManager : ScreenManager
{
    [SerializeField] private LayingManSlotUI _slotPrefab;

    [SerializeField] private Transform _leftContainer;
    [SerializeField] private Transform _rightContainer;

    private List<LayingManSlotUI> _pool = new List<LayingManSlotUI>();
    private List<LayingManSlotUI> _activeSlots = new List<LayingManSlotUI>();

    private TrainController _train;

    public void ShowScreen()
    {
        CollectData();
        Show();
    }

    public void HideScreen()
    {
        ReturnAllToPool();
        Hide();
    }

    public LayingMenOverlayManager Initialize(TrainController train)
    {
        _train = train;
        return this;
    }

    private void CollectData()
    {
        RoadController road = _train.GetCurrentRoad();

        FillContainer(road.LeftRail.LayingMen, _leftContainer);
        FillContainer(road.RightRail.LayingMen, _rightContainer);
    }

    private void FillContainer(List<LayingManController> men, Transform parent)
    {
        for (int i = 0; i < men.Count; i++)
        {
            LayingManSlotUI ui = GetFromPool(parent);
            ui.Set(men[i].Data);
            _activeSlots.Add(ui);
        }
    }

    private LayingManSlotUI GetFromPool(Transform parent)
    {
        if (_pool.Count > 0)
        {
            int lastIndex = _pool.Count - 1;
            LayingManSlotUI ui = _pool[lastIndex];
            _pool.RemoveAt(lastIndex);
            ui.transform.SetParent(parent);
            ui.gameObject.SetActive(true);
            return ui;
        }

        return Instantiate(_slotPrefab, parent);
    }

    private void ReturnAllToPool()
    {
        for (int i = 0; i < _activeSlots.Count; i++)
        {
            LayingManSlotUI ui = _activeSlots[i];
            ui.gameObject.SetActive(false);
            _pool.Add(ui);
        }
        _activeSlots.Clear();
    }
}