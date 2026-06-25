using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class RoadManager : IDisposable
{
    private readonly RoadsConfigSO _config;
    private readonly Transform _parent;
    private readonly RoadFactory _roadFactory;

    private GameStateManager _gameStateManager;
    private TrainController _train;

    private List<RoadController> _roads = new();
    private Vector3 _nextSpawnPosition = Vector3.zero;

    public RoadManager(
        RoadsConfigSO config,
        Transform parent,
        RoadFactory roadFactory)
    {
        _config = config;
        _parent = parent;
        _roadFactory = roadFactory;
    }

    public void Initialize(GameStateManager gameStateManager, TrainController train)
    {
        _gameStateManager = gameStateManager;
        _train = train;

        RoadSelector.SetConfigs(_config.SegmentConfigs);

        for (int i = 0; i < _config.MaxRoads; i++)
        {
            SpawnNextRoad();
        }
    }

    private void SpawnNextRoad()
    {
        RoadSegmentConfigSO segmentConfig = RoadSelector.GetRandom();

        RoadController newRoad = _roadFactory.Get(segmentConfig, _nextSpawnPosition, _parent);
        newRoad.SetDependencies(_train, _gameStateManager);

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
            DestroyOldAndSetNewRoad(road);
        }
    }

    private void DestroyOldAndSetNewRoad(RoadController road)
    {
        _roads.Remove(road);
        _roadFactory.Release(road);
        SpawnNextRoad();
    }

    public void Dispose()
    {
        foreach (var road in _roads)
        {
            if (road != null)
            {
                road.OnRoadStateChanged -= OnRoadStateChanged; 
                _roadFactory.Release(road);
            }
        }
        _roads.Clear();
    }
}