using UnityEngine;
[CreateAssetMenu(fileName = "Attacks")]
public class AttackSO : ScriptableObject
{
    [Header("Damage & KnockBack")]
    public float damage;
    public Vector2 direction;
    public float forceKnockback;
    public float hitStunDuration;
    [Header("Hitbox")]
    public Vector2 offSet;
    public float radius;
    [Header("Momentum")]
    public Vector2 momentumDirection;
    public float momentumForce;
}
