using System.Collections;
using UnityEngine;

public class ArqueroTest : GlassCannon
{
    private enum SubState {Positioning}
    private SubState _subState;
    [SerializeField] private GlassCannonData _glassCannonData;

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
    }

    protected override void HandleCustom()
    {
        switch (_subState)
        {
            case SubState.Positioning:
                HandlePositioning();
                break;

        }
    }

    private void InitialazeStats()
    {
        if (_glassCannonData == null) return;

        MaxHealth = _glassCannonData.maxHealth;
        CurrentHealth = MaxHealth;
        MaxSpeed = _glassCannonData.moveSpeed;
        CurrentSpeed = MaxSpeed;
        CurrentDamage = _glassCannonData.damage;
    }

    protected override void HandleChasing()
    {
        base.HandleChasing();
        float distanceToPlayer = Vector2.Distance(transform.position, _playerRef.transform.position);

        if (distanceToPlayer <= _glassCannonData.attackRange * 0.8f)
            ChangeState(State.Attacking);
        else
            MoveTowardsPlayer();
    }

    public void MoveTowardsPlayer()
    {
        if (!_canMove || _isKnockBack) return;
        float direction = (_playerRef.transform.position.x > transform.position.x) ? 1 : -1;
        _rb.linearVelocity = new Vector2(direction * CurrentSpeed, _rb.linearVelocity.y);
    }

    public void DisableMovement()
    {
        _canMove = false;
        _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y);
    }

    public void EnableMovement()
    {
        _canMove = true;

    }

    public void EndAttack()
    {
        EnableMovement();

        ChangeState(State.Chasing);

        _animator.SetBool("Attacking", false);
        _animator.SetBool("Chasing", true);
    }

    public void HandlePositioning()
    {
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
