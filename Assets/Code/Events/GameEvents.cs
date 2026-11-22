using System;
using UnityEngine;

public enum GameState { Boot, Menu, Generating, Playing, Paused, GameOver }

public static class GameEvents
{
    public static Action<GameState> OnGameStateChanged;
    public static Action OnPlayerDied;
    public static Action OnPlayerHit;
    public static Action OnMaxInvocation;
    public static Action OnPlayerSpawn;

    //LEVELS
    public static Action OnLevelGenerated;
    public static Action OnRequestLevelGeneration;
    
    // Local Rooms
    public static Action<int> OnCombatClusterTriggered;
    public static Action<int> OnRoomRewardCollected;
    public static Action OnEnemyDied;
    
    // Camera Events
    public static Action<Vector3, Vector3> OnEnterCombatCamera;
    public static Action OnExitCombatCamera;
}
