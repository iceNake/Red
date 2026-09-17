using UnityEngine;

[CreateAssetMenu(fileName = "BulkyData", menuName = "Scriptable Objects/BulkyData")]
public class BulkyData : ScriptableObject
{
    [Header("Base Stats")] 
    public float maxHealth = 100f;
    public float damage = 20f;
    public float moveSpeed = 2f;
    
    [Header("Area abilities")]
    public float explosionRadius = 5f;
    public float attackCooldown = 3f;
    public LayerMask playerLayer;
}
