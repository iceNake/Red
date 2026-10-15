using UnityEngine;

public interface IDamageable
{
    void TakeDamage(float damage);

    void TakeKnockback(Vector2 knockbackDirection, float knockbackForce);
}
