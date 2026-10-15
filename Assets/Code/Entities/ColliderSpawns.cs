using System.Collections;
using UnityEngine;

public class ColliderSpawns : MonoBehaviour
{
    public GameObject spawnFront;
    public GameObject spawnBack;
    public bool playerStay;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            spawnFront.SetActive(true);
            spawnBack.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            spawnFront.SetActive(false);
            spawnBack.SetActive(true);
        }
    }
}
