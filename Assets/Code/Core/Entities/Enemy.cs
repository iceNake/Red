using UnityEngine;
using System.Collections;

public enum State { Idle, Chasing, Attacking, Dead }
public class Enemy : Entity
{
    [Header("State Machine")]
    [SerializeField] protected State _currentState = State.Idle;

    protected Player _playerRef;
    public bool isStunned = false;


    [Header("Detection Settings")]
    [SerializeField] protected float _detectionRange = 10f;

    [Header("Stats")]
    public int cost  {get {return _cost;}}
    public int enemyId { get { return _enemyId; } }
    [SerializeField] private int _cost; //QUE NO SEA 0
    [SerializeField] private int _enemyId;//QUE NINGUNO SEA EL MISMO ENTRE ENEMIGOS Y QUE NO SEA 0

    protected override void Awake()
    {
        base.Awake(); 
    }

    protected virtual void OnEnable()
    {
        _playerRef = FindFirstObjectByType<Player>(); 
        if (_playerRef == null) Debug.LogWarning($"Enemy " + gameObject.name + " no encontro a player");
    }
    
    private void OnDisable()
    {
        if(EnemyColliders.instance != null)
        {
            EnemyColliders.instance.ExitCombat(this);
        }
    }

    protected virtual void Update()
    {
        if (!IsAlive || _playerRef == null || isStunned) return;
        
        switch (_currentState)
        {
            case State.Idle:
                HandleIdle();
                break;
            case State.Chasing:
                HandleChasing();
                break;
            case State.Attacking:
                HandleAttacking();
                break;
        }
    }

    protected virtual void HandleIdle()
    {
        
    }

    protected virtual void HandleChasing()
    {
        FlipTowardsPlayer();
    }

    protected virtual void HandleAttacking()
    {
        
    }
    
    public void ChangeState(State newState)
    {
        if (_currentState == newState) return;

        Debug.Log($"{name} {_currentState} {newState}");
        
        _currentState = newState;
        
        switch( _currentState )
        {
            case State.Chasing:
                EnemyColliders.instance?.EnterCombat( this );
                break;
            case State.Dead:
                EnemyColliders.instance?.ExitCombat( this ); 
                break;
        }
       
    }

    protected void FlipTowardsPlayer()
    {
        float direction = _playerRef.transform.position.x - transform.position.x;
        if (direction > 0.1f)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            _playerRef.attackLight.direction = new Vector2(-1, 1);
            _playerRef.attackLight1.direction = new Vector2(-1, 1);
        }
        else if (direction < -0.1f)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            _playerRef.attackLight.direction = new Vector2(1, 1);
            _playerRef.attackLight1.direction = new Vector2(1, 1);
        }
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
    }

    public override void TakeKnockback(Vector2 knockbackDirection, float knockbackForce)
    {
        isStunned = true;
        base.TakeKnockback(knockbackDirection, knockbackForce);
        StartCoroutine(KnockbackStunRoutine(1f));
    }

    public void SetCollisionLayer(bool hasCollision)
    {
        if (hasCollision)
        {
            gameObject.layer = LayerMask.NameToLayer("Enemy");
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("EnemyCollision");
        }

    }

    private IEnumerator KnockbackStunRoutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        isStunned = false;
        _rb.linearVelocity = Vector2.zero;
    }
}
