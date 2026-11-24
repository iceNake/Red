using NaughtyAttributes;
using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Entity : MonoBehaviour, IDamageable
{
    #region Variables
    [SerializeField] private bool _isInHitstun;
    [SerializeField] private Coroutine _hitstunCoroutine;
    public bool IsInHitstun {get; private set;}

    public bool IsPaused {get; private set;}
    private Vector2 _savedVelocity;
    protected Animator _animator;

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
    public bool isPlayerFlipped;
    #endregion

    [Header("Collision Detection")]
    public bool invulnerable = false;



    protected Rigidbody2D _rb;
    protected SpriteRenderer _spriteRenderer;

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
        if(!invulnerable)
        {
            TakeDamage(_damageTest);
            TakeKnockback(_knockbackDirectionTest, _knockbackForceTest);
        }  
    }

    #endregion

    #region Methods

    protected virtual void Awake()
    {
      
        _rb = GetComponent<Rigidbody2D>();
        _rb.freezeRotation = true;
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _animator =  GetComponentInChildren<Animator>();
    }

    protected virtual void OnEnable()
    {
        GameEvents.OnGameStateChanged += HandleGameStateChanged;
        _isAlive = true;
        _currentHealth = _maxHealth;
        SetPaused(false);
    }

    protected virtual void OnDisable()
    {
        GameEvents.OnGameStateChanged -= HandleGameStateChanged;
    }
    
    private void HandleGameStateChanged(GameState newState)
    {
        if (newState == GameState.Paused)
            SetPaused(true);
        else if (newState == GameState.Playing)
            SetPaused(false);
    }
    
    protected virtual void SetPaused(bool paused)
    {
        if (IsPaused == paused) return;
        
        IsPaused = paused;

        if (IsPaused)
        {
            if (_rb != null)
            {
                _savedVelocity = _rb.linearVelocity;
                _rb.simulated = false;
            }
            
            if (_animator != null) _animator.speed = 0f;
        }
        else
        {
            if (_rb != null)
            {
                _rb.simulated = true;
                _rb.linearVelocity = _savedVelocity;
            }
            
            if (_animator != null) _animator.speed = 1f;
        }
    }

    public void Healing(float heal)
    {
        _currentHealth += heal;
        Debug.Log("Ah... I feel better (Healed HP)");
    }

    public virtual void TakeDamage(float damage)
    {
        if(!invulnerable)
        {
            if (_spriteRenderer != null)
            {
                StartCoroutine(SpriteRed());
            }
            _currentHealth -= damage;
            Debug.Log("Ouch! That hurt (Lost HP)");

            if (_currentHealth <= 0)
                Die();
        }
        
    }

    public virtual void TakeKnockback(Vector2 knockbackDirection, float knockbackForce)
    {
        if (!invulnerable)
        {
            Vector2 directionForKnockback = knockbackDirection.normalized;

            _rb.linearVelocity = Vector2.zero;
            _rb.AddForce(directionForKnockback * knockbackForce, ForceMode2D.Impulse);

            Debug.Log("<color=blue> Hey! You knocked the air out of me (Got knockback) </color>");
        }
            
    }

    private IEnumerator SpriteRed()
    {
        if(!invulnerable)
        {
            _spriteRenderer.color = Color.red;

            yield return new WaitForSeconds(0.1f);
            if (_spriteRenderer != null)
            {
                _spriteRenderer.color = Color.white;

            }
        }
        
    }

    public void ApplyHitstun(float duration)
    {
        if(!invulnerable)
        {
            if (!_isAlive) return;

            if (this is Enemy enemy)
            {
                enemy.InterruptAction();
            }

            if (_hitstunCoroutine != null)
            {
                StopCoroutine(_hitstunCoroutine);
            }
            _hitstunCoroutine = StartCoroutine(HitstunCoroutine(duration));
        }
        
    }
    
    public virtual void Die()
    {
        if (!_isAlive) return;

        _isAlive = false;
        gameObject.SetActive(false);
        Debug.Log("My story shouldn't end like this (Died)");
    }

    IEnumerator HitstunCoroutine(float duration)
    {
        _isInHitstun = true;
        yield return WaitWhileNotPaused(duration);

        _isInHitstun = false;
        _hitstunCoroutine = null;

        if(this is Enemy enemy)
        {
            enemy.RecoverFromHitstun();
        }

    }

    public IEnumerator IFrames(float duration)
    {
        invulnerable = true;
        yield return WaitWhileNotPaused(duration);
        invulnerable = false;
    }
    
    protected IEnumerator WaitWhileNotPaused(float duration)
    {
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            if (!IsPaused) elapsed += Time.deltaTime;
            
            yield return null;
        }
    }

    #endregion
}