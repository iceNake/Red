using System;
using UnityEngine;

public class RoomReward : MonoBehaviour
{
    private int _roomID;

    public void Init(int roomID)
    {
        _roomID = roomID;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();

        if (player != null)
        {
            GameEvents.OnRoomRewardCollected?.Invoke(_roomID);
            gameObject.SetActive(false);
        }
    }
}
