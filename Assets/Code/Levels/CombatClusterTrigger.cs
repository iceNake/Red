using System;
using UnityEngine;

public class CombatClusterTrigger : MonoBehaviour
{
    private int _targetClusterID = -1;
    private bool _hasTriggered = false;

    public void Setup(int targetClusterID)
    {
        _targetClusterID = targetClusterID;
        _hasTriggered = false;
        gameObject.SetActive(targetClusterID != -1);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (_hasTriggered || _targetClusterID == -1) return;
        
        Player player = other.GetComponent<Player>();

        if (player != null)
        {
            _hasTriggered = true;
            GameEvents.OnCombatClusterTriggered?.Invoke(_targetClusterID);
        }
    }
}
