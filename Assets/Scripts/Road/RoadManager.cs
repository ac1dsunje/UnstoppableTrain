using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    [SerializeField] private int basicRoadCount = 4;
    [SerializeField] private int choosingRoadCount = 1;

    [SerializeField] private GameObject movingRoadPrefab;
    [SerializeField] private GameObject choosingRoadPrefab;

    [SerializeField] private TrainController _train;

    private List<GameObject> roads;
    private int currentPatternIndex = 0;
    private Vector3 nextSpawnPosition = Vector3.zero;

    private void Awake()
    {
        roads = new List<GameObject>();

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

        GameObject newRoad = Instantiate(prefabToSpawn, nextSpawnPosition, Quaternion.identity);
        roads.Add(newRoad);

        RoadController roadController = newRoad.GetComponent<RoadController>();
        roadController.SetTrainLink(_train);

        roadController.OnRoadStateChanged += OnRoadStateChanged;
        nextSpawnPosition = newRoad.transform.position + new Vector3(0, 0, roadController.RoadLength);

        currentPatternIndex++;
    }

    private void OnRoadStateChanged(GameObject road, bool isActive)
    {
        if (!isActive)
        {
            RoadController roadController = road.GetComponent<RoadController>();
            roadController.OnRoadStateChanged -= OnRoadStateChanged;

            StartCoroutine(DestroyOldAndSetNewRoad(road));
        }
    }

    private IEnumerator DestroyOldAndSetNewRoad(GameObject road)
    {
        yield return new WaitForSeconds(2);

        roads.Remove(road);
        Destroy(road);

        SpawnNextRoad();
    }

    private void OnDestroy()
    {
        foreach (GameObject road in roads)
        {
            if (road != null)
            {
                RoadController roadController = road.GetComponent<RoadController>();
                if (roadController != null)
                {
                    roadController.OnRoadStateChanged -= OnRoadStateChanged;
                }
            }
        }
    }
}