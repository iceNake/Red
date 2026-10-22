using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using Unity.Cinemachine;

public class OsoTest : Bulky
{
    private enum AttackType { Jab, Shove, Slam }
    private AttackType _currentAttack;
    private bool _slamPending;
    private bool _attackHit;
    [SerializeField] private BulkyData _bulkyData;

    [Header("Referencias")]
    [SerializeField] private Animator _animator;
    [SerializeField] private OsoHitbox _attackHitbox;

    [Header("Sprite")]
    [SerializeField] private SpriteRenderer _spriteRenderer;


    private Coroutine _idleRoutine;
    private bool _isIdleWalking;
    private float _idleDirection;

    private float _lastAttackTime;
    private bool _canMove = true;

    protected override void Awake()
    {
        base.Awake();

        _animator = GetComponentInChildren<Animator>();

        InitialazeStats();



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
        float distanceToPlayer = Vector2.Distance(transform.position, _playerRef.transform.position);

        if (distanceToPlayer <= _bulkyData.rangeDetection)
        {
            StopIdleRoutine();

            ChangeState(State.Chasing);
            _animator.SetBool("Chasing", true);
            _animator.SetBool("Attacking", false);
            return;
        }

        if (_idleRoutine == null)
        {
            _idleRoutine = StartCoroutine(IdleRoutine());
        }
        if (_isIdleWalking && _canMove)
        {
            _rb.linearVelocity = new Vector2(_idleDirection * CurrentSpeed, _rb.linearVelocity.y);

            _animator.SetBool("Chasing", true);
        }
    }

    protected override void HandleChasing()
    {
        base.HandleChasing();

        float distanceToPlayer = Vector2.Distance(transform.position, _playerRef.transform.position);

        if (distanceToPlayer <= _bulkyData.rangeAttack)
        {
            /*ChangeState(State.Attacking);
            _animator.SetBool("Attacking", true);
            _animator.SetBool("Chasing", false);*/
            StartAttack();
        }
        else if(distanceToPlayer >= _bulkyData.rangeDetection)
        {
            ChangeState(State.Idle);
            return;
        }
        else
        {
            MoveTowardsPlayer();
        }
    } 

    public void StartAttack()
    {
        if (_slamPending)
        {
            _currentAttack = AttackType.Slam;
            _slamPending = false;
        }
        else
        {
            _currentAttack = Random.value < 0.5f ? AttackType.Jab : AttackType.Shove;
        }
        _attackHit = false;

        ChangeState(State.Attacking);
        DisableMovement();
        _animator.SetBool("Attacking", true);
        _animator.SetBool("Chasing", false);
    }

    private void MoveTowardsPlayer()
    {
       
        if (!_canMove || isStunned) return;

        float direction = (_playerRef.transform.position.x > transform.position.x) ? 1 : -1;

        _rb.linearVelocity = new Vector2(direction * CurrentSpeed, _rb.linearVelocity.y);

    }

    private void FlipTowardsIdleDirection()
    {
        if(_idleDirection > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    public void EnableHitbox()
    {
        if (_attackHitbox == null) return;

        _attackHitbox.EnableCollider();
    }

    public void DisableHitbox()
    {
        if (_attackHitbox != null)
        {
            _attackHitbox.DisableCollider();
        }
    }

    public void JabEnableHitbox()
    {
        if (_currentAttack != AttackType.Jab) return;
        EnableHitbox();
    }
    public void JabDisableHitbox()
    {
        DisableHitbox();
    }
    public void JabEndAttack()
    {
        EndAttack();
    }

    public void ShoveEnableHitbox()
    {
        if (_currentAttack != AttackType.Shove) return;
        EnableHitbox();
    }
    public void ShoveDisableHitbox()
    {
        DisableHitbox();
    }
    public void ShoveEndAttack()
    {
        EndAttack();
    }
    public void SlamEnableHitbox()
    {
        if (_currentAttack != AttackType.Slam) return;
        EnableHitbox();
    }
    public void SlamDisableHitbox()
    {
        DisableHitbox();
    }
    public void SlamEndAttack()
    {
        EndAttack();
    }

    public void OnAttackHit(Player player)
    {
        if (_attackHit) return;

        _attackHit = true;


        if (_currentAttack == AttackType.Jab)
        {
            _slamPending = true;
        }
    }

    public float GetAttackDamage(Player player)
    {
        if (_currentAttack == AttackType.Jab)
            return player.MaxHealth * 0.08f;

        return CurrentDamage;
    }

    public bool IsSlamAttack()
    {
        return _currentAttack == AttackType.Slam;
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
        DisableHitbox();
        EnableMovement();

        _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y);
        ChangeState(State.Chasing);

        _animator.SetBool("Attacking", false);
        _animator.SetBool("Chasing", true);
    }

    private IEnumerator IdleRoutine()
    {
        _isIdleWalking = false;
        _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y);

        yield return new WaitForSeconds(_bulkyData.idleWaitTime);

        _idleDirection = Random.Range(0,2) == 0 ? -1 : 1;
        _isIdleWalking = true;

        FlipTowardsIdleDirection();

        yield return new WaitForSeconds(_bulkyData.idleWalkTime);

        _isIdleWalking = false;
        _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y);
        _animator.SetBool("Chasing", false);
        _idleRoutine = null;
    }

    private void StopIdleRoutine()
    {
        if (_idleRoutine != null)
        {
            StopCoroutine( _idleRoutine);
            _idleRoutine = null;
        }

        _isIdleWalking = false;
        _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _bulkyData.rangeDetection);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _bulkyData.rangeAttack);
    }
}
