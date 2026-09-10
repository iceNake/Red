using UnityEngine;
using NaughtyAttributes;

[RequireComponent(typeof(Rigidbody2D))]
public class Entity : MonoBehaviour, IDamageable
{
    #region Variables

    #region Stats
    [SerializeField] private float _currentHealth;
    [SerializeField] private float _maxHealth;

    [SerializeField] private float _currentSpeed;
    [SerializeField] private float _maxSpeed;

    [SerializeField] private float _currentDamage;
    [SerializeField] private float _maxDamage;
    #endregion

    #region Testing
    [SerializeField] private Vector2 _knockbackDirectionTest;
    [SerializeField] private float _knockbackForceTest;
    [SerializeField] private float _damageTest;
    #endregion

    protected AttackSystem _attackSystem;
    protected Rigidbody2D _rb;

    [SerializeField] private bool _isAlive = true;

    #endregion

    #region Getters & Setters

    public float CurrentHealth { get => _currentHealth; set => _currentHealth = value; }
    public float MaxHealth { get => _maxHealth; set => _maxHealth = value; }

    public float CurrentSpeed { get => _currentSpeed; set => _currentSpeed = value; }
    public float MaxSpeed { get => _maxSpeed; set => _maxSpeed = value; }

    public float CurrentDamage { get => _currentDamage; set => _currentDamage = value; }
    public float MaxDamage { get => _maxDamage; set => _maxDamage = value; }

    public bool IsAlive { get => _isAlive; set => _isAlive = value; }

    #endregion

    #region Methods Testing

    [Button]
    public void TakeDamageAndKnockBackTest()
    {
        TakeDamage(_damageTest);
        TakeKnockback(_knockbackDirectionTest, _knockbackForceTest);
    }

    #endregion

    #region Methods

    protected virtual void Awake()
    {
        if (_attackSystem == null)
        {
            _attackSystem = GetComponent<AttackSystem>();
        }
        _rb = GetComponent<Rigidbody2D>();
        _rb.freezeRotation = true;
    }

    public void Healing(float heal)
    {
        _currentHealth += heal;
        Debug.Log("Ah... I feel better (Healed HP)");
    }

    public virtual void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        Debug.Log("Ouch! That hurt (Lost HP)");

        if (_currentHealth <= 0)
            Die();
    }

    public virtual void TakeKnockback(Vector2 knockbackDirection, float knockbackForce)
    {
        Vector2 directionForKnockback = knockbackDirection.normalized;

        _rb.linearVelocity = Vector2.zero;
        _rb.AddForce(directionForKnockback * knockbackForce, ForceMode2D.Impulse);

        Debug.Log("<color=blue> Hey! You knocked the air out of me (Got knockback) </color>");
    }

    public virtual void Die()
    {
        if (!_isAlive) return;

        _isAlive = false;
        gameObject.SetActive(false);
        Debug.Log("My story shouldn't end like this (Died)");
    }

    #endregion
}