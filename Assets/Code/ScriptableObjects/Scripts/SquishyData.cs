using UnityEngine;

[CreateAssetMenu(fileName = "SquishyData", menuName = "Scriptable Objects/SquishyData")]
public class SquishyData : ScriptableObject
{
    [Header("Base Stats")]
    public float maxHealth = 100f;
    public float damage = 20f;
    public float moveSpeed = 2f;

    [Header("Area abilities")]
    public float attackRange = 5f;
    public float attackCooldown = 3f;
    public LayerMask playerLayer;
}
