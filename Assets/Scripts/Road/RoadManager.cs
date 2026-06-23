using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    [SerializeField] private RoadConfigSO _config;
    private GameStateManager _gameStateManager;
    private TrainController _train;

    private List<RoadController> roads = new();
    private Vector3 nextSpawnPosition = Vector3.zero;
    private static WaitForSeconds _waitFor1Seconds = new WaitForSeconds(1f);

    public RoadManager Initialize(GameStateManager gameStateManager, TrainController train)
    {
        _gameStateManager = gameStateManager;
        _train = train;

        for (int i = 0; i < _config._maxRoads; i++)
        {
            SpawnNextRoad();
        }

        return this;
    }

    private void SpawnNextRoad()
    {
        int rand = Random.Range(0, 100);
        GameObject prefabToSpawn;

        if (rand < _config._choosingRoadChance) prefabToSpawn = _config.choosingRoadPrefab;
        else if (rand < _config._choosingRoadChance + _config._stationRoadChance) prefabToSpawn = _config.stationRoadPrefab;
        else prefabToSpawn = _config.movingRoadPrefab;

        RoadController newRoad = Instantiate(prefabToSpawn, nextSpawnPosition, Quaternion.identity, transform)
            .GetComponent<RoadController>()
            .Initialize(_train, _gameStateManager);

        roads.Add(newRoad);
        newRoad.OnRoadStateChanged += OnRoadStateChanged;

        nextSpawnPosition = newRoad.transform.position + new Vector3(0, 0, newRoad.RoadLength);
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
            StartCoroutine(DestroyOldAndSetNewRoad(road));
        }
    }

    private IEnumerator DestroyOldAndSetNewRoad(RoadController road)
    {
        yield return _waitFor1Seconds;
        roads.Remove(road);
        Destroy(road.gameObject);
        SpawnNextRoad();
    }

    private void OnDestroy()
    {
        foreach (var road in roads)
        {
            if (road != null) road.OnRoadStateChanged -= OnRoadStateChanged;
        }
    }
}