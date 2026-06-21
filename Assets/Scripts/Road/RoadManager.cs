using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;

    [SerializeField] private int basicRoadCount = 4;
    [SerializeField] private int choosingRoadCount = 1;

    [SerializeField] private GameObject movingRoadPrefab;
    [SerializeField] private GameObject choosingRoadPrefab;

    [SerializeField] private TrainController _train;

    private List<RoadController> roads = new();
    private int currentPatternIndex = 0;
    private Vector3 nextSpawnPosition = Vector3.zero;

    private void Awake()
    {
        int totalRoads = basicRoadCount + choosingRoadCount;
        for (int i = 0; i < totalRoads; i++)
        {
            SpawnNextRoad();
        }
    }

    private void SpawnNextRoad()
    {
        int patternLength = basicRoadCount + choosingRoadCount;
        int positionInPattern = currentPatternIndex % patternLength;

        bool isChoosingRoad = positionInPattern >= basicRoadCount;
        GameObject prefabToSpawn = isChoosingRoad ? choosingRoadPrefab : movingRoadPrefab;

        RoadController newRoad = Instantiate(prefabToSpawn, nextSpawnPosition, Quaternion.identity, transform).GetComponent<RoadController>();
        roads.Add(newRoad);

        RoadController roadController = newRoad;
        roadController.SetTrainLink(_train);

        roadController.OnRoadStateChanged += OnRoadStateChanged;
        nextSpawnPosition = newRoad.transform.position + new Vector3(0, 0, roadController.RoadLength);

        currentPatternIndex++;
    }

    private void OnRoadStateChanged(RoadController road, bool isActive)
    {
        if (!isActive)
        {
            road.OnRoadStateChanged -= OnRoadStateChanged;

            StartCoroutine(DestroyOldAndSetNewRoad(road));
        }
        else if (isActive)
        {
            _train.SetCurrentRoad(road);
            switch (road.GetRoadType)
            {
                case RoadType.Choosing:
                    _gameManager.SetChoosingState();
                    break;

                case RoadType.Moving:
                    _gameManager.SetMovingState();
                    break;
            }
        }
            
    }

    private IEnumerator DestroyOldAndSetNewRoad(RoadController road)
    {
        yield return new WaitForSeconds(2);

        roads.Remove(road);
        Destroy(road.gameObject);

        SpawnNextRoad();
    }

    private void OnDestroy()
    {
        foreach (var road in roads)
        {
            if (road != null)
            {
                road.OnRoadStateChanged -= OnRoadStateChanged;
            }
        }
    }
}