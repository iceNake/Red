using UnityEngine;

public class HitBox : MonoBehaviour
{
    private AttackSystem _attackSystem;
    private GameObject _owner;
    public bool hitBool;

    public void Init(AttackSystem system, GameObject ownerObject)
    {
        _attackSystem = system;
        _owner = ownerObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (collision.transform.root.gameObject == _owner)
        {
            return;
        }
        if (damageable == null)
        {
            return;
        }

        damageable.TakeDamage(_attackSystem.damage);
        damageable.TakeKnockback(_attackSystem.dirKnocBack, _attackSystem.forceKnockback);
        Debug.Log(_attackSystem.dirKnocBack + "     " + _attackSystem.forceKnockback +  "AAAAAAAAAAAAAAAAA");
        hitBool = true;

        if (hitBool)
        {
            GameEvents.OnPlayerHit?.Invoke();
            hitBool = false;
        }
    }
}
