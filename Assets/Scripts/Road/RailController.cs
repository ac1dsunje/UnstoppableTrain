using System;
using System.Collections.Generic;
using UnityEngine;

public class RailController : MonoBehaviour
{
    [SerializeField] private GameObject _layingManPrefab;
    private List<LayingManController> _layingMen = new();
    public List<LayingManController> LayingMen => _layingMen;


    public Action<bool> OnThisActive;
    public Action OnAllLayingMenDied;

    public void SpawnManyLayingMen(int count)
    {
        for (int i = 0; i < count; i++) 
        {
            SpawnLayingMan(i);
        }
    }

    private void SpawnLayingMan(int step)
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z + step * 2);
        LayingManController _layingMan = Instantiate(_layingManPrefab, pos, Quaternion.identity, transform).GetComponent<LayingManController>();
        _layingMen.Add(_layingMan);
        _layingMan.OnDeath += OnLayingManDeath;
    }

    private void OnDestroy()
    {
        foreach (var item in _layingMen)
        {
            item.OnDeath -= OnLayingManDeath;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Train")
        {
            SetRailActive();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Train")
        {
            SetRailUnActive();
        }
    }

    private void SetRailActive()
    {
        OnThisActive.Invoke(true);
    }

    private void SetRailUnActive()
    {
        OnThisActive.Invoke(false);
    }

    private void OnLayingManDeath(LayingManController layingMan)
    {
        _layingMen.Remove(layingMan);

        if (_layingMen.Count == 0)
        {
            OnAllLayingMenDied.Invoke();
        }
    }
}
