using UnityEngine;
using System.Collections.Generic;

public class EnemySpawningSystem
{
    private EnemyBundleSO _enemies; 
    private LevelManager _levelManager;
    
    public EnemySpawningSystem(EnemyBundleSO enemy, LevelManager levelManager)
    {
        _enemies = enemy;
        _levelManager = levelManager;
    }
    
    public int SpawnEnemies(int budget, List<Transform> availableSpawnPoints)
    {
        if (_enemies == null || _enemies.enemy.Length == 0 || availableSpawnPoints.Count == 0) return 0;

        int remainingBudget = budget;
        int spawnedCount = 0;
        
        List<Transform> unusedSpawns = new List<Transform>(availableSpawnPoints);
        
        int minCost = int.MaxValue;
        
        foreach (GameObject prefab in _enemies.enemy)
        {
            Enemy enemyScript = prefab.GetComponent<Enemy>();
            if (enemyScript != null && enemyScript.cost > 0 && enemyScript.cost < minCost)
            {
                minCost = enemyScript.cost;
            }
        }
        
        while (remainingBudget >= minCost && unusedSpawns.Count > 0)
        {
            int randomIndex = Random.Range(0, _enemies.enemy.Length);
            GameObject selectedEnemyPrefab = _enemies.enemy[randomIndex];
            Enemy enemyScript = selectedEnemyPrefab.GetComponent<Enemy>();

            if (enemyScript.cost <= remainingBudget)
            {
                int randomSpawnIndex = Random.Range(0, unusedSpawns.Count);
                Transform spawnPoint = unusedSpawns[randomSpawnIndex];
                
                _levelManager.SpawnEnemyFromPool(selectedEnemyPrefab, spawnPoint);

                remainingBudget -= enemyScript.cost;
                unusedSpawns.RemoveAt(randomSpawnIndex);
                
                spawnedCount++;
            }
        }

        return spawnedCount;
    }
}
