using System;
using UnityEngine;

public class RoomReward : MonoBehaviour
{
    private int _clusterID;

    public void Init(int clusterID)
    {
        _clusterID = clusterID;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();

        if (player != null)
        {
            GameEvents.OnRoomRewardCollected?.Invoke(_clusterID);
            gameObject.SetActive(false);
            // aqui hay que hacer que se active el canvas para lo de la eleccion del reward
        }
    }
}
