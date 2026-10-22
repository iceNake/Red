using System.Collections;
using UnityEngine;

public class ArqueroTest : GlassCannon
{
    private enum SubState {Positioning}
    private SubState _subState;
    [SerializeField] private GlassCannonData _glassCannonData;

    [Header("Referencias")]
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _arrowPrefab;
    [SerializeField] private Transform _shootPoint;

    [Header("Posicionamiento")]
    [SerializeField] private float _positionTolerance = 0.3f;


    [Header("Sprite")]
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [SerializeField] private Vector2 leftKnockBack = new Vector2(-1, 0f);
    [SerializeField] private Vector2 rightKnockBack = new Vector2(1, 0f);

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

    protected override void HandleIdle()
    {
        if (Vector2.Distance(transform.position, _playerRef.transform.position) < _detectionRange)
        {
            _animator.SetBool("Chasing", true);
            ChangeState(State.Chasing);
            _animator.SetBool("Attacking", false);
            StartCoroutine(ShootRoutine());
        }
    }

    protected override void HandleChasing()
    {
        base.HandleChasing();
        float distanceToPlayer = Vector2.Distance(transform.position, _playerRef.transform.position);

        if (distanceToPlayer <= _glassCannonData.attackRange)
        {
            _subState = SubState.Positioning;
            _animator.SetBool("Attacking", false);
            _animator.SetBool("Chasing", true);
            ChangeState(State.Custom);

        }
        else
        {
            MoveTowardsPlayer();
        }
    }

    public void MoveTowardsPlayer()
    {
        if (!_canMove || isStunned) return;
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
        _animator.SetBool("Attacking", false);
        _animator.SetBool("Chasing", true);
        ChangeState(State.Custom);
    }

    public void HandlePositioning()
    {
        float playerX = _playerRef.transform.position.x;
        float myX = transform.position.x;

        float distance = Mathf.Abs(playerX - myX);
        float dirToPlayer = Mathf.Sign(playerX - myX);

        float error = distance - _glassCannonData.attackRange;



        if (Mathf.Abs(error) > _positionTolerance)
        {
            float moveDir = error > 0 ? dirToPlayer : -dirToPlayer;
            _rb.linearVelocity = new Vector2(moveDir * CurrentSpeed, _rb.linearVelocity.y);
        }
        else
        {
            _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y);
        }

    }

    public void Shoot()
    {
        if (_playerRef == null) return;

        Vector2 direction = (_playerRef.transform.position - _shootPoint.position).normalized;

        GameObject projectile = Instantiate(_arrowPrefab, _shootPoint.position, Quaternion.identity);

        if (projectile.TryGetComponent<EnemyProjectile>(out EnemyProjectile proj))
        {
            proj.Init(direction, CurrentDamage);
        }
    }


    public override void TakeDamage(float damage)
    {
        StartCoroutine(SpriteRed());
        base.TakeDamage(damage);
    }

    private IEnumerator SpriteRed()
    {
        _spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        _spriteRenderer.color = Color.white;
    }



    private IEnumerator ShootRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_glassCannonData.attackCD);
            ChangeState(State.Attacking);
            _animator.SetBool("Attacking", true);
            _animator.SetBool("Chasing", false);

        }
    }
}
