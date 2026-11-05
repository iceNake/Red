using UnityEngine;

public class ZonaMuerte : MonoBehaviour
{
    private void OnEnable()
    {
        GameEvents.OnPlayerDied += HandlePlayerDie;
    }
    private void OnDisable()
    {
        GameEvents.OnPlayerDied -= HandlePlayerDie;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            HandlePlayerDie();
        }
    }

    private void HandlePlayerDie()
    {
        GameEvents.OnPlayerDied?.Invoke();
    }
}
