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
            else
            {
                finalMap.Add(SelectFromPool(slot.type));
            }
        }
        
        Debug.Log($"<color=green><b>[LevelGenerator]</b> Ruta construida. Total de habitaciones guardadas: {finalMap.Count}</color>");
        for (int i = 0; i < finalMap.Count; i++)
        {
            if (finalMap[i] != null)
            {
                Debug.Log($"Habitación [{i}]: {finalMap[i].name}");
            }
            else
            {
                Debug.LogWarning($"Habitación [{i}]: ¡Alerta! El objeto es nulo (revisa las pools o prefabs fijos).");
            }
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