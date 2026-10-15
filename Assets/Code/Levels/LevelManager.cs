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
    
    // este diccionario guarda un pool independiente para cada tipo de habitacion.
    private Dictionary<GameObject, ObjectPool<GameObject>> roomPools = new Dictionary<GameObject, ObjectPool<GameObject>>();
    
    // aqui estan las habitaciones que estan prendidas
    private List<GameObject> activeRooms = new List<GameObject>();

    [Header("Enemy Generation")]
    [SerializeField] private EnemyBundleSO _enemyBundle;
    //Juan porfa haz un script que almacene la informacion de cada nivel como en este caso sus spawnpoints
    [SerializeField] private List<Transform> _spawnPoints = new List<Transform>();
    [SerializeField] private int _budget;
    private EnemySpawningSystem _enemySpawningSystem;
    private EnemySpawnPointsContainer _enemySpawnPointsContainer;
    
    private void Awake()
    {
        if (_enemyBundle != null) _enemySpawningSystem = new EnemySpawningSystem(_enemyBundle, this);
    }

    private void OnEnable()
    {
        GameEvents.OnRequestLevelGeneration += LevelGeneration;
    }

    private void OnDisable()
    {
        GameEvents.OnRequestLevelGeneration -= LevelGeneration;
    }
    
    /// <summary>
    /// el async void permirte que el metodo se conecte al evento
    /// tambien permite que dentro se puedan ejecutar tareas como la pausa (await).
    /// Barrera si lees esto "Hola".
    /// </summary>
    private async void LevelGeneration()
    {
        if (levelGenerator != null)
        {
            ClearLevel();
            levelGenerator.BuildRoute();
            
            // aqui el await nos sirve para que esto se quede pausado hasta que el metodo
            // BuildLevelAsync termine
            await BuildLevelAsync(levelGenerator.finalMap);
            
            await Awaitable.WaitForSecondsAsync(2.5f);
            
            GameEvents.OnLevelGenerated?.Invoke();
            _enemySpawningSystem.SpawnEnemies(_budget, _spawnPoints);
        }
        else
        {
            Debug.LogError("[LevelGenerator] No esta asignado en el isnpector ponlooo");
        }
    }

    /// <summary>
    /// el Awaitable nos ayudara a que el metoodo haga trabajo asincrono, esto nos ayuda
    /// a no usar corrutinas.
    /// </summary>
    private async Awaitable BuildLevelAsync(List<GameObject> roomPrefabs)
    {
        _enemySpawnPointsContainer = GetComponent<EnemySpawnPointsContainer>();
        Transform lastExit = startingPoint;

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

            EnemySpawnPointsContainer spawnPointsContainer = roomInstance.GetComponent<EnemySpawnPointsContainer>();
            if (spawnPointsContainer != null)
            {
                for (int i = 0; i < spawnPointsContainer._enemyLevelspawnPoints.Count; i++)
                {
                    _spawnPoints.Add(spawnPointsContainer._enemyLevelspawnPoints[i]);
                }
            }

            // esto es como magia negra
            // el await hace que se pause el bucle durante un frame, esto nos sirve para que el
            // juego no se congele y la pantalla de carga siga con su animacion de manera fluida.
            await Awaitable.NextFrameAsync();
        }

    }

    private GameObject GetRoomFromPool(GameObject prefab)
    {
        // el if pregunta si no hay un almacen en el diccionario del tipo de mapa que queremos
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
        
        _spawnPoints.Clear();
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
    
    /*public void SpawnEnemy(GameObject enemy, Transform spawnPoint)
    {
        Instantiate(enemy, spawnPoint);
    }*/
}
