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

        LayingManController layingMan = _layingManFactory.Get(pos, transform);

        _layingMen.Add(layingMan);
        layingMan.OnDeath += OnLayingManDeath;
    }

    public void ClearLayingMen()
    {
        foreach (var man in _layingMen)
        {
            man.OnDeath -= OnLayingManDeath;
            _layingManFactory.Release(man);
        }
        _layingMen.Clear();
    }

    private void OnDisable()
    {
        ClearLayingMen();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Train"))
        {
            OnThisActive?.Invoke(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Train"))
        {
            OnThisActive?.Invoke(false);
        }
    }

    private void OnLayingManDeath(LayingManController layingMan)
    {
        layingMan.OnDeath -= OnLayingManDeath;

        _layingMen.Remove(layingMan);
        _layingManFactory.Release(layingMan);

        if (_layingMen.Count == 0)
        {
            OnAllLayingMenDied?.Invoke();
        }
    }
}