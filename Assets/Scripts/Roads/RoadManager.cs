using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;
using Object = UnityEngine.Object;

public class RoadManager : IDisposable
{
    private readonly RoadsConfigSO _config;
    private readonly MonoBehaviour _coroutineRunner;
    private readonly Transform _parent;
    private readonly ManGeneralConfigSO _manConfig;
    private readonly ManVisualConfigSO _manVisualConfig;

    private GameStateManager _gameStateManager;
    private TrainController _train;

    private List<RoadController> _roads = new();
    private Vector3 _nextSpawnPosition = Vector3.zero;
    private static readonly WaitForSeconds _waitFor1Seconds = new WaitForSeconds(1f);

    public RoadManager(
        RoadsConfigSO config,
        MonoBehaviour coroutineRunner,
        Transform parent,
        ManGeneralConfigSO manConfig,
        ManVisualConfigSO manVisualConfig)
    {
        _config = config;
        _coroutineRunner = coroutineRunner;
        _parent = parent;
        _manConfig = manConfig;
        _manVisualConfig = manVisualConfig;
    }

    public void Initialize(GameStateManager gameStateManager, TrainController train)
    {
        _gameStateManager = gameStateManager;
        _train = train;

        for (int i = 0; i < _config.MaxRoads; i++)
        {
            SpawnNextRoad();
        }
    }

    private void SpawnNextRoad()
    {
        int rand = Random.Range(0, 100);
        GameObject prefabToSpawn;

        if (rand < _config.ChoosingRoadChance) prefabToSpawn = _config.ChoosingRoadPrefab;
        else if (rand < _config.ChoosingRoadChance + _config.StationRoadChance) prefabToSpawn = _config.StationRoadPrefab;
        else prefabToSpawn = _config.MovingRoadPrefab;

        RoadController newRoad = Object.Instantiate(prefabToSpawn, _nextSpawnPosition, Quaternion.identity, _parent)
            .GetComponent<RoadController>()
            .Initialize(_train, _gameStateManager, _manConfig, _manVisualConfig);

        _roads.Add(newRoad);
        newRoad.OnRoadStateChanged += OnRoadStateChanged;

        _nextSpawnPosition = newRoad.transform.position + new Vector3(0, 0, newRoad.RoadLength);
    }

    private void OnRoadStateChanged(RoadController road, bool isActive)
    {
        if (isActive)
        {
            _train.SetCurrentRoad(road);
        }
        else
        {
            road.OnRoadStateChanged -= OnRoadStateChanged;
            _coroutineRunner.StartCoroutine(DestroyOldAndSetNewRoad(road));
        }
    }

    private IEnumerator DestroyOldAndSetNewRoad(RoadController road)
    {
        yield return _waitFor1Seconds;
        _roads.Remove(road);
        Object.Destroy(road.gameObject);
        SpawnNextRoad();
    }

    public void Dispose()
    {
        foreach (var road in _roads)
        {
            if (road != null)
            {
                road.OnRoadStateChanged -= OnRoadStateChanged;
            }
        }
        _roads.Clear();
    }
}