using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "EscenarioConfig", menuName = "Juego/ScenarioData")]
public class ScenarioData : ScriptableObject
{
    [Header("Estructura de la Partida")]
    public List<LevelSlot> levelSequence;

    [Header("Pools de Variedad")]
    public List<GameObject> combatRoomsPool;
    public List<GameObject> eventRoomsPool;
    public List<GameObject> defaultRoomsPool;
}
