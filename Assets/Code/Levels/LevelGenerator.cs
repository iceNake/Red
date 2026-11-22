using UnityEngine;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
    public ScenarioData config;
    public List<GameObject> finalMap = new List<GameObject>();

    private const int COMBAT_CLUSTER_SIZE = 2;
    
    public void BuildRoute()
    {
        finalMap.Clear();

        if (config == null) return;
        
        if (config.startRoomPrefab != null) finalMap.Add(config.startRoomPrefab);
        
        if (config.defaultRoomsPool.Count > 0) finalMap.Add(SelectFromPool(RoomType.Default));

        for (int block = 0; block < config.combatBlocksCount; block++)
        {
            List<GameObject> tempCombatPool = new List<GameObject>(config.combatRoomsPool);

            for (int i = 0; i < COMBAT_CLUSTER_SIZE; i++)
            {
                if (tempCombatPool.Count == 0) break;
                
                int randomIndex = Random.Range(0, tempCombatPool.Count);
                finalMap.Add(tempCombatPool[randomIndex]);
                tempCombatPool.RemoveAt(randomIndex);
            }
            
            if (config.eventRoomsPool != null && config.eventRoomsPool.Count > 0 && Random.value < config.eventRoomChance)
                finalMap.Add(SelectFromPool(RoomType.Event));
            
            if (config.defaultRoomsPool.Count > 0) finalMap.Add(SelectFromPool(RoomType.Default));
        }
        
        if (config.bossRoomPrefab != null) finalMap.Add(config.bossRoomPrefab);

        Debug.Log($"<color=green><b>[LevelGenerator]</b> Ruta procedural construida. Total de habitaciones: {finalMap.Count}</color>");
        
        for (int i = 0; i < finalMap.Count; i++)
        {
            if (finalMap[i] != null) Debug.Log($"Habitación [{i}]: {finalMap[i].name}");
        }
    }
    
    GameObject SelectFromPool(RoomType type)
    {
        switch (type)
        {
            case RoomType.Combat:
                return config.combatRoomsPool[Random.Range(0, config.combatRoomsPool.Count)];
            case RoomType.Event:
                return config.eventRoomsPool[Random.Range(0, config.eventRoomsPool.Count)];
            case RoomType.Default:
                return config.defaultRoomsPool[Random.Range(0, config.defaultRoomsPool.Count)];
            default:
                return null;
        }
    }
}