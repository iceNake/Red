using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class RoomManager : MonoBehaviour
{
    [Header("Room ID")]
    public int roomID;

    [Header("Enemy Local Spawn")]
    [SerializeField] private int _roomBudget;
    [SerializeField] private EnemyBundleSO _enemyBundle;
    public List<Transform> enemyLevelSpawnPoints = new List<Transform>();
    
    [Header("Reward Settings")]
    [SerializeField] private Transform _rewardSpawnPoint;
    [SerializeField] private GameObject _rewardPrefab;

    private int _activeEnemiesCount;
    private EnemySpawningSystem _spawningSystem;
    private LevelManager _levelManager;
    private bool _isRoomActive = false;
    private bool _isCleared = false;

    public void Init(int id, LevelManager manager)
    {
        roomID = id;
        _levelManager = manager;
        
        if (_enemyBundle != null) _spawningSystem = new EnemySpawningSystem(_enemyBundle, _levelManager);
    }

    private void OnEnable()
    {
        GameEvents.OnRoomRewardCollected += HandleRewardCollected;
        GameEvents.OnEnemyDied += HandleEnemyDied;
        GameEvents.OnLevelGenerated += HandleLevelStart;
    }

    private void OnDisable()
    {
        GameEvents.OnRoomRewardCollected -= HandleRewardCollected;
        GameEvents.OnEnemyDied -= HandleEnemyDied;
        GameEvents.OnLevelGenerated -= HandleLevelStart;
    }

    private void HandleLevelStart()
    {
        if (roomID == 0) StartRoom();
    }

    private void HandleRewardCollected(int completedRoomID)
    {
        if (roomID == completedRoomID + 1) StartRoom();
    }

    private void StartRoom()
    {
        if (_isCleared || _isRoomActive) return;
        _isRoomActive = true;

        if (_roomBudget <= 0 || enemyLevelSpawnPoints.Count == 0 || _enemyBundle == null || _spawningSystem == null)
        {
            SpawnReward();
            return;
        }

        _activeEnemiesCount = _spawningSystem.SpawnEnemies(_roomBudget, enemyLevelSpawnPoints);
        
        if (_activeEnemiesCount <= 0) SpawnReward();
    }

    private void HandleEnemyDied()
    {
        if (!_isRoomActive || _isCleared) return;

        _activeEnemiesCount--;
        
        if (_activeEnemiesCount <= 0) SpawnReward();
    }

    private void SpawnReward()
    {
        _isCleared = true;
        _isRoomActive = false;

        if (_rewardSpawnPoint != null && _rewardSpawnPoint != null)
        {
            GameObject reward = Instantiate(_rewardPrefab, _rewardSpawnPoint.position, Quaternion.identity);
            
            if (reward.TryGetComponent<RoomReward>(out RoomReward roomReward)) roomReward.Init(roomID);
        }
    }
}
