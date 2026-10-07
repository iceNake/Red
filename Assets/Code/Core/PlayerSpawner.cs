using UnityEngine;
using Unity.Cinemachine;

public class PlayerSpawner : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform spawnPoint;
    
    private GameObject currentPlayerInstance;

    private void OnEnable()
    {
        GameEvents.OnPlayerSpawn += HandlePlayerSpawn;
    }
    
    private void OnDisable()
    {
        GameEvents.OnPlayerSpawn -= HandlePlayerSpawn;
    }

    private void HandlePlayerSpawn()
    {
        if (playerPrefab == null)
        {
            Debug.LogError("No hay un player asignado");
            return;
        }
        
        Transform targetSpawn = spawnPoint != null ? spawnPoint : transform;
        
        if (currentPlayerInstance != null) Destroy(currentPlayerInstance);
        
        currentPlayerInstance = Instantiate(playerPrefab, targetSpawn.position, targetSpawn.rotation);
        AssignPlayerToCamera(currentPlayerInstance.transform);
        Debug.Log("<color=green><b>[PlayerSpawner]</b> Jugador instanciado exitosamente.</color>");
    }
    
    private void AssignPlayerToCamera(Transform playerTransform)
    {
        CinemachineCamera cam = Object.FindFirstObjectByType<CinemachineCamera>();
        
        if (cam != null)
        {
            cam.Follow = playerTransform;
            cam.LookAt = playerTransform; 
        }
        else
        {
            Debug.LogWarning("[PlayerSpawner] No se encontró ninguna CinemachineCamera en la escena.");
        }
    }
}
