using UnityEngine;

public class OsoHitbox : MonoBehaviour
{
    private OsoTest _oso;
    private Collider2D _collider;

    public void Initialized(OsoTest oso)
    {
        _oso = oso;
    }

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    public void EnableCollider()
    {
        _collider.enabled = true;
    }

    public void DisableCollider()
    {
        _collider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        Player player = collision.GetComponentInParent<Player>();

        if (player == null)
            return;

        if (_oso == null)
            return;

        player.TakeDamage(_oso.CurrentDamage);
    }
}
