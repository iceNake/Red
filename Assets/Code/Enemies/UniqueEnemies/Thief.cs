using UnityEngine;
using UnityEngine.Events;

public class Thief : GlassCannon
{
    [SerializeField] private GlassCannonData _glassCannonData;

    [Header("Events")]
    public UnityEvent OnRangeAttack;
    public UnityEvent OnTakeDamage;
    public GameObject projectilePrefab;
    public Transform firePoint;
    private float _lastAttackTime;
    [SerializeField] private Vector2 leftKnockBack = new Vector2(-1, 0f);
    [SerializeField] private Vector2 rightKnockBack = new Vector2(1, 0f);

    protected override void Awake()
    {
        base.Awake();
        InitializeStats();
        leftKnockBackDirection = leftKnockBack;
        rightKnockBackDirection = rightKnockBack;
    }
    //Pasamos las variables del scriptableobject para el enemigo
    private void InitializeStats()
    {
        if (_glassCannonData == null) return;

        MaxHealth = _glassCannonData.maxHealth;
        CurrentHealth = MaxHealth;
        MaxSpeed = _glassCannonData.moveSpeed;
        CurrentSpeed = MaxSpeed;
        CurrentDamage = _glassCannonData.damage;
    }
    //Aqui este sirve para cambiar el idle para chasing cuando la distancia entre el player y el enemigo sea menor del attack range del enemigo
    protected override void HandleIdle()
    {
        if (Vector2.Distance(transform.position, _playerRef.transform.position) < _glassCannonData.attackRange) ChangeState(State.Chasing);
    }
    //En este metodo agarramos la distancia del player y la restamos con la posicion del enemigo, si la distancia del player es menor o igual al range attack del enemigo va a cambiar al estado de ataque, si no va a seguirse moviendo
    protected override void HandleChasing()
    {
        base.HandleChasing();
        float distanceToPlayer = Vector2.Distance(transform.position, _playerRef.transform.position);

        if (distanceToPlayer <= _glassCannonData.attackRange * 0.8f)
            ChangeState(State.Attacking);
        else
            MoveTowardsPlayer();
    }
    //Aqui atacamos pero primero le bajamos la velocidad de movimiento al enemigo para que se quede parado y en el if si lastattack time + glasscannon attack cd es menor o igual a time.time hacemos perform y cambiamos a estado chasing
    protected override void HandleAttacking()
    {
        _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y);

        if (Time.time >= _lastAttackTime + _glassCannonData.attackCD)
        {
            PerformAoEAttack();
            ChangeState(State.Chasing);
        }
    }
    //Aqui recibimos da�o
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        OnTakeDamage?.Invoke();
    }
    //Aqui solo cambiamos la direccion del enemigo hacia un lado o el otro dependiendo de donde se encuentre el player y le damos la velocidad
    private void MoveTowardsPlayer()
    {
        float direction = (_playerRef.transform.position.x > transform.position.x) ? 1 : -1;
        _rb.linearVelocity = new Vector2(direction * CurrentSpeed, _rb.linearVelocity.y);
    }
    //Aqui estamos usando un overlapCircleAll que funciona como un raycast de circulo asi que si esta dentro del circulo el enemigo va a ahcer da�o
    private void PerformAoEAttack()
    {
        _lastAttackTime = Time.time;
        OnRangeAttack?.Invoke();

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, _glassCannonData.attackRange, _glassCannonData.playerLayer);
        
        foreach (var col in hitColliders)
        {

            if (col.TryGetComponent<Player>(out Player p))
            {
                ShootProjectile(p);
            }
        }
    }
    // gizmo osea pintar
    private void OnDrawGizmosSelected()
    {
        if (_glassCannonData == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _glassCannonData.attackRange);
    }

    private void ShootProjectile(Player target)
    {
        if (target == null) return;

        Vector2 direction = (target.transform.position - firePoint.position).normalized;

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        if (projectile.TryGetComponent<EnemyProjectile>(out EnemyProjectile proj))
        {
            proj.Init(direction, CurrentDamage);
        }
    }

    private void RangeEscape()
    {
    }
    
    
}
