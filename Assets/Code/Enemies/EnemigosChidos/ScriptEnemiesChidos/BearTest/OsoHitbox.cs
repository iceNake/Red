using UnityEngine;

public class OsoHitbox : MonoBehaviour
{
    private OsoTest _oso;
    private Collider2D _collider;
    [SerializeField] private float _knockbackForce = 8f;
    private bool _isSlam;
    private bool _hasHit;

    public void Initialized(OsoTest oso)
    {
        _oso = oso;
    }

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _collider.enabled = false;
    }
    public void SetSlam(bool isSlam)
    {
        _isSlam = isSlam;
    }

    public void EnableCollider()
    {
        _hasHit = false;
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
        if (_hasHit) return;

        _hasHit = true; 

        Vector2 direction = (player.transform.position - _oso.transform.position).normalized;
        direction.y = 0.3f;
        direction.Normalize();

        player.TakeDamage(_oso.GetAttackDamage(player));
        _oso.OnAttackHit(player);

        if (_oso.IsSlamAttack())
        {
            player.TakeKnockback(direction, _knockbackForce);

        }
    }
}
