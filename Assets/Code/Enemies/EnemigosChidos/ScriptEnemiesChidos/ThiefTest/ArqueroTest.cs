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

    [Header("Personal Space")]
    [SerializeField] private float _personalSpace = 1.5f;

    private bool _canMove = true;
    
    private Coroutine _shootCoroutine;
    private Coroutine _colorCoroutine;

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
            
            if (_shootCoroutine != null) StopCoroutine(_shootCoroutine);
            
            _shootCoroutine = StartCoroutine(ShootRoutine());
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
        
        LookAtPlayer();
        float dirToPlayer = (_playerRef.transform.position.x > transform.position.x) ? 1 : -1;

        if (IsArcheryInFront(dirToPlayer))
        {
            _rb.linearVelocity = new Vector2(-dirToPlayer * (CurrentSpeed * 0.5f), _rb.linearVelocity.y);
            return;
        }
        
        _rb.linearVelocity = new Vector2(dirToPlayer * CurrentSpeed, _rb.linearVelocity.y);
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
        if (IsInHitstun) return;
        
        LookAtPlayer();
        float playerX = _playerRef.transform.position.x;
        float myX = transform.position.x;

        float distance = Mathf.Abs(playerX - myX);
        float dirToPlayer = Mathf.Sign(playerX - myX);

        float error = distance - _glassCannonData.attackRange;

        if (IsArcheryInFront(dirToPlayer))
        {
            _rb.linearVelocity = new Vector2(-dirToPlayer * (CurrentSpeed * 0.5f), _rb.linearVelocity.y);
            return;
        }

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
        
        LookAtPlayer();
        float dirX = Mathf.Sign(_playerRef.transform.position.x - transform.position.x);
        Vector2 horizontalDirection = new Vector2(dirX, 0f);
        GameObject projectile = Instantiate(_arrowPrefab, _shootPoint.position, Quaternion.identity);

        if (projectile.TryGetComponent<EnemyProjectile>(out EnemyProjectile proj))
        {
            proj.Init(horizontalDirection, CurrentDamage);
            
            if (dirX < 0)
                projectile.transform.localScale = new Vector3(-10f, 2f, 1f);
            else
                projectile.transform.localScale = new Vector3(10f, 2f, 1f);
        }
    }


    public override void TakeDamage(float damage)
    {
        if (_colorCoroutine != null) StopCoroutine(_colorCoroutine);
        
        _colorCoroutine = StartCoroutine(SpriteRed());
        base.TakeDamage(damage);
    }

    private IEnumerator SpriteRed()
    {
        _spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        _spriteRenderer.color = Color.white;
        _colorCoroutine = null;
    }

    public override void RecoverFromHitstun()
    {
        _animator.SetBool("Stunned", false);
        _animator.SetBool("Attacking", false);
        _animator.SetBool("Chasing", true);

        EnableMovement();

        ChangeState(State.Chasing);
        
        if (_shootCoroutine != null) StopCoroutine(_shootCoroutine);
        
        _shootCoroutine = StartCoroutine(ShootRoutine());
    }

    public override void InterruptAction()
    {
        base.InterruptAction();

        DisableMovement();
        
        if (_shootCoroutine != null)
        {
            StopCoroutine(_shootCoroutine);
            _shootCoroutine = null;
        }

        _animator.SetBool("Attacking", false);
        _animator.SetBool("Chasing", false);
        _animator.SetBool("Stunned", true);
    }

    private bool WatchingPlayer()
    {
        if (_playerRef == null) return false;

        float dirX = Mathf.Sign(_playerRef.transform.position.x - transform.position.x);
        Vector2 horizontalDirection = new Vector2(dirX, 0f);
        RaycastHit2D[] hits = Physics2D.RaycastAll(_shootPoint.position, horizontalDirection, _glassCannonData.attackRange);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject == this.gameObject) continue;

            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy")) continue;
            
            if (hit.collider.GetComponent<Player>() != null) return true; 
            
            return false; 
        }
        
        return false;
    }

    private IEnumerator ShootRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_glassCannonData.attackCD);

            if (WatchingPlayer())
            {
                ChangeState(State.Attacking);
                _animator.SetBool("Attacking", true);
                _animator.SetBool("Chasing", false);
            }
        }
    }

    private void LookAtPlayer()
    {
        if (_playerRef == null) return;
        
        float dirX = Mathf.Sign(_playerRef.transform.position.x - transform.position.x);
        
        if (dirX > 0f)
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        else if (dirX < 0f)
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
    }

    private bool IsArcheryInFront(float dirX)
    {
        Vector2 direction = new Vector2(dirX, 0f);
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, _personalSpace);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject == this.gameObject) continue;
            
            if (hit.collider.GetComponent<ArqueroTest>() != null) return true;
        }
        
        return false;
    }
}
