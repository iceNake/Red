using UnityEngine;
[CreateAssetMenu(fileName = "Attacks")]
public class AttackSO : ScriptableObject
{
    [Header("Damage & KnockBack")]
    public float damage;
    public Vector2 direction;
    public float forceKnockback;
    public float durationAttack;
    public float hitStunDuration;
    [Header("Hitbox")]
    public Vector2 offSet;
    public float radius; 
}
