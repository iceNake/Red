using UnityEngine;

public class RoomConnector : MonoBehaviour
{
    [SerializeField] private Transform entrance;
    [SerializeField] private Transform exit;
    
    public Transform Entrance => entrance;
    public Transform Exit => exit;
}
