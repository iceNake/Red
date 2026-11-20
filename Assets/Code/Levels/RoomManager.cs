using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class RoomManager : MonoBehaviour
{
    [Header("Room Identity")]
    public bool isCombatRoom = true;
    public int clusterID = -1;
    public bool isFirstInCluster;
    public bool isLastInCluster;

    [Header("Enemy Local Spawn")]
    [SerializeField] private int _roomBudget;
    [SerializeField] private EnemyBundleSO _enemyBundle;
    public List<Transform> enemyLevelSpawnPoints = new List<Transform>();

    [Header("Reward Settings")]
    [SerializeField] private GameObject _wallPrefab;
    [SerializeField] private Transform _rewardSpawnPoint;
    [SerializeField] private GameObject _rewardPrefab;

    private EnemySpawningSystem _spawningSystem;
    private LevelManager _levelManager;
    private RoomConnector _roomConnector;

    private GameObject _activeEntranceWall;
    private GameObject _activeExitWall;

    private int _totalClusterEnemiesAlive;
    private bool _isClusterActive = false;
    private bool _isClusterCleared = false;

    private void Awake()
    {
        _roomConnector = GetComponent<RoomConnector>();
    }

    public void InitCombatRoom(int cID, bool isFirts, bool isLast, LevelManager manager)
    {
        isCombatRoom = true;
        clusterID = cID;
        isFirstInCluster = isFirts;
        isLastInCluster = isLast;
        _levelManager = manager;
        
        _isClusterActive = false;
        _isClusterCleared = false;
        _totalClusterEnemiesAlive = 0;
        
        if (_enemyBundle != null) _spawningSystem = new EnemySpawningSystem(_enemyBundle, _levelManager);
    }

    private void OnEnable()
    {
        GameEvents.OnCombatClusterTriggered += HandleClusterTriggered;
        GameEvents.OnRoomRewardCollected += HandleRewardCollected;
        GameEvents.OnEnemyDied += HandleEnemyDied;
    }

    private void OnDisable()
    {
        GameEvents.OnCombatClusterTriggered -= HandleClusterTriggered;
        GameEvents.OnRoomRewardCollected -= HandleRewardCollected;
        GameEvents.OnEnemyDied -= HandleEnemyDied;

        DestroyWalls();
    }

    private void HandleClusterTriggered(int triggeredClusterID)
    {
        if (!isCombatRoom || clusterID != triggeredClusterID || _isClusterActive || _isClusterCleared) return;

        _isClusterActive = true;
        int mySpawnedEnemies = 0;

        if (_roomBudget > 0 && enemyLevelSpawnPoints.Count > 0 && _enemyBundle != null && _spawningSystem != null)
            mySpawnedEnemies = _spawningSystem.SpawnEnemies(_roomBudget, enemyLevelSpawnPoints);

        if (_wallPrefab != null && _roomConnector != null)
        {
            if (isFirstInCluster && _roomConnector.Entrance != null)
                _activeEntranceWall = Instantiate(_wallPrefab, _roomConnector.Entrance.transform.position, _roomConnector.Entrance.transform.rotation);
            
            if (isLastInCluster && _roomConnector.Exit != null)
                _activeExitWall = Instantiate(_wallPrefab, _roomConnector.Exit.transform.position, _roomConnector.Exit.transform.rotation);
        }
        
        if  (mySpawnedEnemies > 0) _levelManager.RegisterEnemiesToCluster(clusterID, mySpawnedEnemies);
    }

    public void AddEnemiesToClusterCount(int count)
    {
        _totalClusterEnemiesAlive += count;
    }
    
    private void HandleEnemyDied()
    {
        if (!isCombatRoom || !isLastInCluster || !_isClusterActive || _isClusterCleared) return;

        _totalClusterEnemiesAlive--;
        
        if (_totalClusterEnemiesAlive <= 0) SpawnReward();
    }
    
    private void SpawnReward()
    {
        _isClusterCleared = true;

        if (_rewardSpawnPoint != null && _rewardSpawnPoint != null)
        {
            GameObject reward = Instantiate(_rewardPrefab, _rewardSpawnPoint.position, Quaternion.identity);
            
            if (reward.TryGetComponent<RoomReward>(out RoomReward roomReward)) roomReward.Init(clusterID);
        }
    }
    
    private void HandleRewardCollected(int completedClusterID)
    {
        if (!isCombatRoom || clusterID != completedClusterID) return;
        
        _isClusterCleared = false;
        DestroyWalls();
    }

    private void DestroyWalls()
    {
        if (_activeEntranceWall != null) Destroy(_activeEntranceWall);
        if (_activeExitWall != null) Destroy(_activeExitWall);
    }
}
