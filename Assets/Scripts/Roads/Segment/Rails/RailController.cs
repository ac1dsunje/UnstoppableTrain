using System;
using System.Collections.Generic;
using UnityEngine;

public class RailController : MonoBehaviour
{
    private LayingManFactory _layingManFactory;

    private List<LayingManController> _layingMen = new();
    public List<LayingManController> LayingMen => _layingMen;

    public event Action<bool> OnThisActive;
    public event Action OnAllLayingMenDied;

    public RailController Initialize(LayingManFactory layingManFactory)
    {
        _layingManFactory = layingManFactory;
        return this;
    }

    public void SpawnManyLayingMen(int count)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnLayingMan(i);
        }
    }

    private void SpawnLayingMan(int step)
    {
        Vector3 pos = new Vector3(
            transform.position.x,
            transform.position.y,
            transform.position.z + step * 2
        );

        LayingManController layingMan = _layingManFactory.Create(pos, transform);

        _layingMen.Add(layingMan);
        layingMan.OnDeath += OnLayingManDeath;
    }

    private void OnDestroy()
    {
        foreach (var item in _layingMen)
        {
            item.OnDeath -= OnLayingManDeath;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Train"))
        {
            SetRailActive();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Train"))
        {
            SetRailUnActive();
        }
    }

    private void SetRailActive() => OnThisActive?.Invoke(true);
    private void SetRailUnActive() => OnThisActive?.Invoke(false);

    private void OnLayingManDeath(LayingManController layingMan)
    {
        _layingMen.Remove(layingMan);

        if (_layingMen.Count == 0)
        {
            OnAllLayingMenDied?.Invoke();
        }
    }
}