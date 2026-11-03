using System;
using UnityEngine;

public class Tree : MonoBehaviour
{
    [SerializeField] private int targetLayerIndex;

    private void Start()
    {
        targetLayerIndex = LayerMask.NameToLayer("Attack");
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        HitBox hitbox = other.GetComponent<HitBox>();
        
        if (other.gameObject.layer == targetLayerIndex)
        {
            if (hitbox != null)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
