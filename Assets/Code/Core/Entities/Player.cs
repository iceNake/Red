using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public enum AttackDirection { Up, Down, Right, Left, Neutral }
public class Player : Entity
{
    [Header("Animator Hashes")]
    private static readonly int HorizontalHash = Animator.StringToHash("Horizontal");
    private static readonly int VerticalHash = Animator.StringToHash("Vertical");
    private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");
    private static readonly int JumpHash = Animator.StringToHash("Jump");
    private static readonly int RedButtonHash = Animator.StringToHash("REDButton");
    private static readonly int DodgeHash = Animator.StringToHash("Dodge");

    [Header("Input Settings")]
    public InputActionAsset actions;
    private InputAction _move;
    private InputAction _jump;
    private InputAction _red;
    private InputAction _dash;
    private InputAction _interact;

    [Header("Movement Settings")]
    public float walkSpeed = 8f;
    public float jumpSpeed = 12f;
    public float dashForce = 20f;
    public bool stunned;

    private bool isDashing;
    private Vector2 _moveInput;
    [SerializeField] private bool isGrounded;

    [Header("Detection Settings")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float interactionRadius = 1.5f;
    [SerializeField] private LayerMask interactableLayer;

    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Attack SO")]
    public AttackSO attackLight;
    public AttackSO attackLight1;
    
    private Animator _animator;

    private void OnEnable()
    {
        if (actions != null)
        {
            var map = actions.FindActionMap("Player");
            if (map != null) map.Enable();
        }
    }

    private void OnDisable()
    {
        if (actions != null)
        {
            var map = actions.FindActionMap("Player");
            if (map != null) map.Disable();
        }
    }

    protected override void Awake()
    {
        base.Awake();
        
        var map = actions.FindActionMap("Player");
        
        _move = map.FindAction(PlayerStrings.PlayerInputStrings.move);
        _jump = map.FindAction(PlayerStrings.PlayerInputStrings.jump);
        _red = map.FindAction(PlayerStrings.PlayerInputStrings.red);
        _dash = map.FindAction(PlayerStrings.PlayerInputStrings.dash);
        _interact = map.FindAction(PlayerStrings.PlayerInputStrings.interact);
    }

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (stunned) return;
        InputRead();
        UpdateAnimatorParameters();
        Flip();
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        ApplyMovement();
    }

    public void InputRead()
    {
        _moveInput = _move.ReadValue<Vector2>();

        if (_jump.WasPressedThisFrame())
        {
            Jump();
        }

        if (_red.WasPressedThisFrame()) 
        {
            AttackDirection dir = GetAttackDir(_moveInput);
            if (_attackSystem != null) _attackSystem.Attack(isGrounded, dir);
            _animator.SetTrigger(RedButtonHash);
        }

        if (_dash.WasPressedThisFrame())
        {
            Dash();
        }

        if (_interact.WasPressedThisFrame())
        {
            PerformInteraction();
        }
    }

    private void ApplyMovement()
    {
        if (isDashing) return;

        float horizontalSpeed = _moveInput.x * walkSpeed;
        float currentVerticalVelocity = _rb.linearVelocity.y;

        _rb.linearVelocity = new Vector2(horizontalSpeed, currentVerticalVelocity);
    }

    public void Jump()
    {
        if (isGrounded)
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, 0); 
            _rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
            
            _animator.SetTrigger(JumpHash);
        }
    }

    public void Dash()
    {
        if (Mathf.Abs(_moveInput.x) > 0.1f)
        {
            isDashing = true;

            float dashDirection = Mathf.Sign(_moveInput.x);
            _rb.linearVelocity = new Vector2(dashDirection * dashForce, 0);

            Invoke(nameof(EndDash), 0.2f);
        }

        _animator.ResetTrigger(DodgeHash);
        _animator.SetTrigger(DodgeHash);
    }

    void EndDash()
    {
        isDashing = false;
    }

    private void UpdateAnimatorParameters()
    {
        _animator.SetFloat(HorizontalHash, Mathf.Abs(_moveInput.x));
        _animator.SetFloat(VerticalHash, _moveInput.y);
        _animator.SetBool(IsGroundedHash, isGrounded);
    }

    private void Flip()
    {
        if (_moveInput.x > 0)
        {
            Debug.Log("derecha");
            spriteRenderer.flipX = false;
        }
        else if (_moveInput.x < 0)
        {
            Debug.Log("izquierda");
            spriteRenderer.flipX = true;
        }
    }

    AttackDirection GetAttackDir(Vector2 input)
    {
        // Umbral de 0.5f para evitar lecturas accidentales del joystick
        if (input.y > 0.5f) return AttackDirection.Up;
        if (input.y < -0.5f) return AttackDirection.Down;
        if (input.x > 0.5f) return AttackDirection.Right;
        if (input.x < -0.5f) return AttackDirection.Left;
        return AttackDirection.Neutral;
    }

    private void PerformInteraction()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, interactionRadius, interactableLayer);
        if (hit != null) 
        {
            Debug.Log("Interactuando con: " + hit.name);
            // Aquí iría la lógica de interacción (ej. hit.GetComponent<IInteractable>().Interact();)
            hit.GetComponent<IInteractable>().Interact();
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Visualizar el radio de interacción en el Editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);

        // Visualizar el Ground Check
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    public override void Die()
    {
        base.Die();
        GameEvents.OnPlayerDied?.Invoke();
    }

    public void OnStun(float duration)
    {
        StartCoroutine(StunCoroutine(duration));
    }
    private IEnumerator StunCoroutine(float duration)
    {
        stunned = true;
        Debug.Log("Stun en proceso");
        yield return new WaitForSeconds(duration);
        stunned = false;
        Debug.Log("Ya no estoy estuneado");
    }
    
}

public static class PlayerStrings
{
    public static class PlayerInputStrings
    {
        public const string move = "Move";
        public const string red = "RED";
        public const string jump = "Jump";
        public const string dash = "Dash";
        public const string interact = "Interact";
    }
}