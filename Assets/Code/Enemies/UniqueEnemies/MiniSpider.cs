using UnityEngine;
using UnityEngine.Events;

public class MiniSpider : Squishy
{
    [SerializeField] private SquishyData _squishyData;

    [Header("Events")]
    public UnityEvent OnExplosionAttack;
    public UnityEvent OnTakeDamage;

    [Header("Referencias")]
    [SerializeField] private Animator _animator;

    private float _lastAttackTime;
    
    [SerializeField] private Vector2 leftKnockBack = new Vector2(-1, 0f);
    [SerializeField] private Vector2 rightKnockBack = new Vector2(1, 0f);

    protected override void Awake()
    {
        _animator = GetComponent<Animator>();
        base.Awake();
        InitializeStats();
        leftKnockBackDirection = leftKnockBack;
        rightKnockBackDirection = rightKnockBack;
    }

    private void InitializeStats()
    {
        if (_squishyData == null) return;

        MaxHealth = _squishyData.maxHealth;
        CurrentHealth = MaxHealth;
        MaxSpeed = _squishyData.moveSpeed;
        CurrentSpeed = MaxSpeed;
        CurrentDamage = _squishyData.damage;
    }

    protected override void HandleIdle()
    {
        if (Vector2.Distance(transform.position, _playerRef.transform.position) < _detectionRange)
        {
            ChangeState(State.Chasing);
            _animator.SetBool("Chasing", true);
            _animator.SetBool("Attacking", false);
        }

    }

    protected override void HandleChasing()
    {
        base.HandleChasing();
        float distanceToPlayer = Vector2.Distance(transform.position, _playerRef.transform.position);

        if (distanceToPlayer <= _squishyData.attackRange * 0.8f)
        {
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

        if (Time.time >= _lastAttackTime + _squishyData.attackCooldown)
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

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, _squishyData.attackRange, _squishyData.playerLayer);
        foreach (var col in hitColliders)
        {
            if (col.TryGetComponent<Player>(out Player p))
            {
                p.TakeDamage(CurrentDamage);
                Debug.Log("Squishy hit: " + p.name);
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
        if (_squishyData == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _squishyData.attackRange);
    }
}
