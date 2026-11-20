using UnityEngine;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
    public ScenarioData config;
    public List<GameObject> finalMap = new List<GameObject>();
    
    public void BuildRoute()
    {
        finalMap.Clear();

        foreach (LevelSlot slot in config.levelSequence)
        {
            if (slot.isRequired && slot.fixedPrefab != null)
            {
                finalMap.Add(slot.fixedPrefab);
            }
            else if (slot.type == RoomType.Combat)
            {
                int clusterSize = Random.Range(2, 4);
                
                List<GameObject> tempCombatPool = new List<GameObject>(config.combatRoomsPool);

                for (int i = 0; i < clusterSize; i++)
                {
                    if (tempCombatPool.Count == 0) break;
                    
                    int randomIndex = Random.Range(0, tempCombatPool.Count);
                    finalMap.Add(tempCombatPool[randomIndex]);
                    
                    tempCombatPool.RemoveAt(randomIndex);
                }
            }
            else
            {
                finalMap.Add(SelectFromPool(slot.type));
            }
        }
        
        Debug.Log($"<color=green><b>[LevelGenerator]</b> Ruta construida. Total de habitaciones guardadas: {finalMap.Count}</color>");
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