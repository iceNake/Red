using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using Unity.Cinemachine;

public class OsoTest : Bulky
{
    [SerializeField] private BulkyData _bulkyData;

    [Header("Events")]
    public UnityEvent OnExplosionAttack;
    public UnityEvent OnTakeDamage;

    [Header("Referencias")]
    [SerializeField] private Animator _animator;
    [SerializeField] private Collider2D _attackHitbox;

    [Header("Sprite")]
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [SerializeField] private Vector2 leftKnockBack = new Vector2(-1, 0f);
    [SerializeField] private Vector2 rightKnockBack = new Vector2(1, 0f);

    private float _lastAttackTime;
    private bool _canMove = true;

    protected override void Awake()
    {
        base.Awake();

        _animator = GetComponentInChildren<Animator>();

        InitialazeStats();

        leftKnockBackDirection = leftKnockBack;
        rightKnockBackDirection = rightKnockBack;

        if (_attackHitbox != null)
            _attackHitbox.enabled = false;
        
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
        if (!_canMove) return;

        float direction = (_playerRef.transform.position.x > transform.position.x) ? 1 : -1;

        _rb.linearVelocity = new Vector2(direction * CurrentSpeed, _rb.linearVelocity.y);

    }

    public void EnableHitbox()
    {
        if (_attackHitbox != null)
        {
            _attackHitbox.enabled = true;
        }

        Debug.Log("Oso HitboxActivada");
    }

    public void DisableHitbox()
    {
        if (_attackHitbox != null)
        {
            _attackHitbox.enabled = false;
        }

        Debug.Log("Oso HitboxDesactivada");
    }

    public void DisableMovement()
    {
        _canMove = false;
        _rb.linearVelocity = new Vector2 (0, _rb.linearVelocity.y);

        Debug.Log("Oso NoMover");
    }

    public void EnableMovement()
    {
        _canMove = true;

        Debug.Log("Oso SiMover");
    }

    public void EndAttack()
    {
        DisableHitbox();
        EnableMovement();

        ChangeState(State.Chasing);

        _animator.SetBool("Attacking", false);
        _animator.SetBool("Chasing", true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out Player player))
        {
            player.TakeDamage(CurrentDamage);
            Debug.Log("Le hize " + CurrentDamage );
        }
    }

    public override void TakeDamage(float damage)
    {
        StartCoroutine(SpriteRed());
        base.TakeDamage(damage);

        OnTakeDamage?.Invoke();
        Debug.Log("<color=blue> Oso Tomo daño</color>");
    }

    private IEnumerator SpriteRed()
    {
        _spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        _spriteRenderer.color = Color.white;
    }





}
