using UnityEngine;

[CreateAssetMenu(fileName = "GlassCannonData", menuName = "Scriptable Objects/GlassCannonData")]
public class GlassCannonData : ScriptableObject
{
    [Header("Base Stats")]
    public float maxHealth = 30f;
    public float damage = 50f;
    public float moveSpeed = 2f;
    
    [Header("Ability")]
    public LayerMask playerLayer;
    public float attackCD = 2f;
    public float attackRange = 20f;
}
