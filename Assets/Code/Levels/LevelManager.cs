using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Pool;

public class LevelManager : MonoBehaviour
{ 
    [Header("Level References")]
    [SerializeField] private Transform startingPoint;
    [SerializeField] private LevelGenerator levelGenerator;

    [Header("Hierarchy Organization")]
    [SerializeField] private Transform poolContainer;
    [SerializeField] private Transform activeLevelContainer;

    [Header("Enemy Pooling")]
    [SerializeField] private Transform enemyPoolContainer;
    [SerializeField] private Transform activeEnemiesContainer;
    
    private Dictionary<GameObject, ObjectPool<GameObject>> enemyPools = new Dictionary<GameObject, ObjectPool<GameObject>>();
    private List<GameObject> activeEnemies = new List<GameObject>();
    private Dictionary<GameObject, ObjectPool<GameObject>> roomPools = new Dictionary<GameObject, ObjectPool<GameObject>>();
    private List<GameObject> activeRooms = new List<GameObject>();

    private void OnEnable()
    {
        GameEvents.OnRequestLevelGeneration += LevelGeneration;
        GameEvents.OnPlayerDied += HandleDespawnEnemies;
    }

    private void OnDisable()
    {
        GameEvents.OnRequestLevelGeneration -= LevelGeneration;
        GameEvents.OnPlayerDied -= HandleDespawnEnemies;
    }
    
    private async void LevelGeneration()
    {
        if (levelGenerator != null)
        {
            ClearLevel();
            levelGenerator.BuildRoute();
            
            await BuildLevelAsync(levelGenerator.finalMap);
            
            await Awaitable.WaitForSecondsAsync(2.5f);
            
            GameEvents.OnLevelGenerated?.Invoke();
        }
        else
        {
            Debug.LogError("[LevelGenerator] No esta asignado en el isnpector ponlooo");
        }
    }
    
    private async Awaitable BuildLevelAsync(List<GameObject> roomPrefabs)
    {
        Transform lastExit = startingPoint;
        int currentRoomID = 0;

        foreach (GameObject prefab in roomPrefabs)
        {
            if (prefab == null) continue;
            
            GameObject roomInstance = GetRoomFromPool(prefab);
            roomInstance.transform.SetParent(activeLevelContainer);
            roomInstance.SetActive(true);
            activeRooms.Add(roomInstance);
            RoomConnector connector = roomInstance.GetComponent<RoomConnector>();
            
            if (connector == null)
            {
                Debug.LogError($" {prefab.name} no tiene el componente RoomConnector.");
                continue;
            }
            
            Vector3 targetPosition = (lastExit != null) ? lastExit.position : Vector3.zero;
            Vector3 entranceOffset = connector.Entrance.position - roomInstance.transform.position;
            roomInstance.transform.position = targetPosition - entranceOffset;
            lastExit = connector.Exit;

            RoomManager roomManager = roomInstance.GetComponent<RoomManager>();
            
            if (roomManager != null) roomManager.Init(currentRoomID, this);
            
            currentRoomID++;
            
            await Awaitable.NextFrameAsync();
        }

    }

    private GameObject GetRoomFromPool(GameObject prefab)
    {
        if (!roomPools.ContainsKey(prefab))
        {
            roomPools[prefab] = new ObjectPool<GameObject>(
                createFunc: () => Instantiate(prefab, poolContainer),
                actionOnGet: obj => obj.gameObject.SetActive(true),
                actionOnRelease: obj =>
                {
                    obj.SetActive(false);
                    obj.transform.SetParent(poolContainer);
                },
                actionOnDestroy: obj => Destroy(obj),
                defaultCapacity: 10,
                maxSize: 50
            );
        }
        
        return roomPools[prefab].Get();
    }

    private void ClearLevel()
    {
        for (int i = 0; i < activeRooms.Count; i++)
        {
            foreach (var kvp in roomPools)
            {
                if (activeRooms[i].name.Contains(kvp.Key.name))
                {
                    kvp.Value.Release(activeRooms[i]);
                    break;
                }
            }
        }
        activeRooms.Clear();
        HandleDespawnEnemies();
    }

    public void SpawnEnemyFromPool(GameObject enemyPrefab, Transform spawnPoint)
    {
        if (!enemyPools.ContainsKey(enemyPrefab))
        {
            enemyPools[enemyPrefab] = new ObjectPool<GameObject>(
                createFunc: () => Instantiate(enemyPrefab, enemyPoolContainer),
                actionOnGet: obj => obj.gameObject.SetActive(true),
                actionOnRelease: obj =>
                {
                    obj.SetActive(false);
                    obj.transform.SetParent(enemyPoolContainer);
                },
                actionOnDestroy: obj => Destroy(obj),
                defaultCapacity: 20,
                maxSize: 100
            );
        }

        GameObject enemyInstance = enemyPools[enemyPrefab].Get();
        enemyInstance.transform.SetParent(activeEnemiesContainer);
        enemyInstance.transform.position = spawnPoint.position;
        enemyInstance.transform.rotation = spawnPoint.rotation;
        activeEnemies.Add(enemyInstance);
    }

    private void HandleDespawnEnemies()
    {
        for (int i = 0; i < activeEnemies.Count; i++)
        {
            foreach (var kvp in enemyPools)
            {
                if (activeEnemies[i].name.Contains(kvp.Key.name))
                {
                    kvp.Value.Release(activeEnemies[i]);
                    break;
                }
            }
        }
        activeEnemies.Clear();
    }
}
