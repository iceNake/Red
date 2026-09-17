using UnityEngine;

public class HitboxWeapon : MonoBehaviour
{
    AttackSystem attackSystem;

    public void Awake()
    {
        attackSystem = GetComponentInParent<AttackSystem>();
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(attackSystem.damage);
            damageable.TakeKnockback(attackSystem.dirKnocBack, attackSystem.forceKnockback);
        }
    }
}
