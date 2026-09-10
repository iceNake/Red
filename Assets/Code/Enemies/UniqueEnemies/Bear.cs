using UnityEngine;
using UnityEngine.Events;

public class Bear : Bulky
{
    [SerializeField] private BulkyData _bulkyData;

    [Header("Events")]
    public UnityEvent OnExplosionAttack;
    public UnityEvent OnTakeDamage;

    [Header("Referencias")]
    [SerializeField] private Animator _animator;

    private float _lastAttackTime;

    protected override void Awake()
    {
        base.Awake();
        _animator = GetComponent<Animator>();
        InitializeStats();
    }

    private void InitializeStats()
    {
        if (_bulkyData == null) return;

        MaxHealth = _bulkyData.maxHealth;
        CurrentHealth = MaxHealth;
        MaxSpeed = _bulkyData.moveSpeed;
        CurrentSpeed = MaxSpeed;
        CurrentDamage = _bulkyData.damage;
    }

    protected override void HandleIdle()
    {
        if (Vector2.Distance(transform.position, _playerRef.transform.position) < _detectionRange)
        {
            _animator.SetBool("Chasing", true);
            ChangeState(State.Chasing);
            Debug.Log("<color=red> Bear Chasing</color>");
            _animator.SetBool("Attacking", false);
        }

    }

    protected override void HandleChasing()
    {
        base.HandleChasing();
        float distanceToPlayer = Vector2.Distance(transform.position, _playerRef.transform.position);

        if (distanceToPlayer <= _bulkyData.explosionRadius * 0.8f)
        {
            Debug.Log("Entro a atacar");
            ChangeState(State.Attacking);
            _animator.SetBool("Attacking", true);
            _animator.SetBool("Chasing", false);

        }


        else
            MoveTowardsPlayer();
    }

    protected override void HandleAttacking()
    {
        _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y);

        if (Time.time >= _lastAttackTime + _bulkyData.attackCooldown)
        {
            PerformAoEAttack();
            ChangeState(State.Chasing);
            _animator.SetBool("Chasing", true);
            _animator.SetBool("Attacking", false);

        }
    }

    private void MoveTowardsPlayer()
    {
        float direction = (_playerRef.transform.position.x > transform.position.x) ? 1 : -1;
        _rb.linearVelocity = new Vector2(direction * CurrentSpeed, _rb.linearVelocity.y);
    }

    private void PerformAoEAttack()
    {
        _lastAttackTime = Time.time;
        OnExplosionAttack?.Invoke();

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, _bulkyData.explosionRadius, _bulkyData.playerLayer);
        foreach (var col in hitColliders)
        {
            if (col.TryGetComponent<Player>(out Player p))
            {
                p.TakeDamage(CurrentDamage);
                Debug.Log("Bulky hit: " + p.name);
            }
        }
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        OnTakeDamage?.Invoke();
    }

    private void OnDrawGizmosSelected()
    {
        if (_bulkyData == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _bulkyData.explosionRadius);
    }
}
