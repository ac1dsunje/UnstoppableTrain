using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;

    [SerializeField] private int _choosingRoadChance;
    [SerializeField] private int _maxRoads;

    [SerializeField] private GameObject movingRoadPrefab;
    [SerializeField] private GameObject choosingRoadPrefab;

    [SerializeField] private TrainController _train;

    private List<RoadController> roads = new();
    private Vector3 nextSpawnPosition = Vector3.zero;

    private static WaitForSeconds _waitFor1Seconds = new WaitForSeconds(1f);

    private void Awake()
    {
        for (int i = 0; i < _maxRoads; i++)
        {
            SpawnNextRoad();
        }
    }

    private void SpawnNextRoad()
    {
        int rand = Random.Range(0, 100);

        GameObject prefabToSpawn = rand < _choosingRoadChance ? choosingRoadPrefab : movingRoadPrefab;

        RoadController newRoad = Instantiate(prefabToSpawn, nextSpawnPosition, Quaternion.identity, transform).GetComponent<RoadController>();
        roads.Add(newRoad);

        newRoad.SetTrainLink(_train);

        newRoad.OnRoadStateChanged += OnRoadStateChanged;
        nextSpawnPosition = newRoad.transform.position + new Vector3(0, 0, newRoad.RoadLength);
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
                    _gameManager.SetSocialState();
                    break;
            }
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
            if (road != null)
            {
                road.OnRoadStateChanged -= OnRoadStateChanged;
            }
        }
    }
}