using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using Unity.Cinemachine;

public class OsoTest : Bulky
{
    [SerializeField] private BulkyData _bulkyData;

    [Header("Referencias")]
    [SerializeField] private Animator _animator;
    [SerializeField] private OsoHitbox _attackHitbox;

    [Header("Sprite")]
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [SerializeField] private Vector2 leftKnockBack = new Vector2(-1, 0f);
    [SerializeField] private Vector2 rightKnockBack = new Vector2(1, 0f);

    private bool _isKnockBack;
    [SerializeField] private float _knockBackDuration = 0.25f;

    private float _lastAttackTime;
    private bool _canMove = true;

    protected override void Awake()
    {
        base.Awake();

        _animator = GetComponentInChildren<Animator>();

        InitialazeStats();

        leftKnockBackDirection = leftKnockBack;
        rightKnockBackDirection = rightKnockBack;

        _attackHitbox = GetComponentInChildren<OsoHitbox>();
        _attackHitbox.Initialized(this);

        if (_attackHitbox != null)
        {
            _attackHitbox.Initialized(this);
            _attackHitbox.enabled = false;
        }
        
    }

    private void InitialazeStats()
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
            _animator.SetBool("Attacking", false);
        }
    }

    protected override void HandleChasing()
    {
        base.HandleChasing();

        float distanceToPlayer = Vector2.Distance(transform.position, _playerRef.transform.position);

        if (distanceToPlayer <= _bulkyData.explosionRadius * 0.8f)
        {
            ChangeState(State.Attacking);
            _animator.SetBool("Attacking", true);
            _animator.SetBool("Chasing", false);
        }
        else
        {
            MoveTowardsPlayer();
        }
    }

    private void MoveTowardsPlayer()
    {
        if (!_canMove || _isKnockBack) return;

        float direction = (_playerRef.transform.position.x > transform.position.x) ? 1 : -1;

        _rb.linearVelocity = new Vector2(direction * CurrentSpeed, _rb.linearVelocity.y);

    }

    public void EnableHitbox()
    {
        if (_attackHitbox != null)
        {
            _attackHitbox.EnableCollider();
        }
    }

    public void DisableHitbox()
    {
        if (_attackHitbox != null)
        {
            _attackHitbox.DisableCollider();
        }
    }

    public void DisableMovement()
    {
        _canMove = false;
        _rb.linearVelocity = new Vector2 (0, _rb.linearVelocity.y);
    }

    public void EnableMovement()
    {
        _canMove = true;
    }

    public void EndAttack()
    {
        DisableHitbox();
        EnableMovement();

        ChangeState(State.Chasing);

        _animator.SetBool("Attacking", false);
        _animator.SetBool("Chasing", true);
    }

    public override void TakeDamage(float damage)
    {
        StartCoroutine(SpriteRed());
        base.TakeDamage(damage);

        StartCoroutine(KnockBackRoutine());

        Debug.Log("<color=blue> Oso Tomo daño</color>");
    }

    private IEnumerator SpriteRed()
    {
        _spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        _spriteRenderer.color = Color.white;
    }

    private IEnumerator KnockBackRoutine()
    {
        _isKnockBack = true;
        yield return new WaitForSeconds(_knockBackDuration);
        _isKnockBack = false;
    }





}
