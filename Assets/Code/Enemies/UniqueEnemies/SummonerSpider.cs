using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class SummonerSpider : Summoner
{
    [SerializeField] private SummonerData _summonerData;

    [Header("Events")]
    public UnityEvent OnInvokeAttack;
    public UnityEvent OnTakeDamage;

    [Header("Referencias")]
    [SerializeField] private Animator _animator;

    [Header("Invocaciones")]
    public bool canStun;
    public Transform pointInvocations;
    public GameObject invocations;

    private float _lastAttackTime;

    [SerializeField] private Vector2 leftKnockBack = new Vector2(-1, 0f);
    [SerializeField] private Vector2 rightKnockBack = new Vector2(1, 0f);
    protected override void Awake()
    {
        canStun = true;
        _animator = GetComponent<Animator>();
        base.Awake();
        InitializeStats();
        leftKnockBackDirection = leftKnockBack;
        rightKnockBackDirection = rightKnockBack;
    }

    private void InitializeStats()
    {
        if (_summonerData == null) return;

        MaxHealth = _summonerData.maxHealth;
        CurrentHealth = MaxHealth;
        MaxSpeed = _summonerData.moveSpeed;
        CurrentSpeed = MaxSpeed;
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

        if (distanceToPlayer <= _summonerData.attackInvoke * 0.8f)
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

        if (Time.time >= _lastAttackTime + _summonerData.attackCooldown)
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
        OnInvokeAttack?.Invoke();

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, _summonerData.attackInvoke, _summonerData.playerLayer);
        foreach (var col in hitColliders)
        {
            if (col.TryGetComponent<Player>(out Player p))
            {
                Instantiate(invocations, this.gameObject.transform.position, this.gameObject.transform.rotation);
                if (canStun)
                {
                    p.OnStun(_summonerData.stunTime);
                    Debug.Log("summon: " + p.name);
                }
                else if (!canStun)
                {
                    StartCoroutine(StunSummonCoroutine());
                }
            }
        }
    }
    IEnumerator StunSummonCoroutine()
    {
        yield return new WaitForSeconds(_summonerData.stunCooldown);
        Debug.Log(this.gameObject + "INVOKE" + invocations);
        canStun = true;
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        OnTakeDamage?.Invoke();
    }

    private void OnDrawGizmosSelected()
    {
        if (_summonerData == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _summonerData.attackInvoke);
    }
}
