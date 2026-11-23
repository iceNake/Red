using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "EscenarioConfig", menuName = "Juego/ScenarioData")]
public class ScenarioData : ScriptableObject
{
    [Header("Fixed Rooms")]
    public GameObject startRoomPrefab;
    public GameObject bossRoomPrefab;

    [Header("Procedural Configuration")]
    public int combatBlocksCount = 3;
    public float eventRoomChance = 0.2f;

    [Header("Pools de Variedad")]
    public List<GameObject> combatRoomsPool;
    public List<GameObject> eventRoomsPool;
    public List<GameObject> defaultRoomsPool;
}
