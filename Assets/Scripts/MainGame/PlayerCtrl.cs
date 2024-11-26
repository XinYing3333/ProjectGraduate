using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCtrl : MonoBehaviour
{
    [Header("Input Settings")]
    public PlayerInput playerInput; // Input System PlayerInput
    public Animator anim;           // Player's animator
    public Transform cameraTransform;
    private Rigidbody _rb;

    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed = 10.0f;
    [SerializeField] private float turnSpeed = 20f;
    [SerializeField] private float gravityNum = 30f;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 10f;

    private CombatSystem _combatSystem;

    // InputAction references
    private InputAction _movement;
    private InputAction _jump;
    private InputAction _lightAttack;
    private InputAction _heavyAttack;


    private Vector3 _rawInputMovement;
    private bool _isMove;
    private bool _isJumping;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _combatSystem = GetComponent<CombatSystem>();
        playerInput = GetComponent<PlayerInput>();

        // Assign InputActions
        _movement = playerInput.actions.FindAction("Movement");
        _jump = playerInput.actions.FindAction("Jump");
        _lightAttack = playerInput.actions.FindAction("LightAttack");
        _heavyAttack = playerInput.actions.FindAction("HeavyAttack");


        // Bind input actions
        _jump.performed += ctx => OnJump();
        _lightAttack.performed += ctx => OnLightAttack();
        _heavyAttack.performed += ctx => OnHeavyAttack();


        // Hide the cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        OnMovement();
    }

    void FixedUpdate()
    {
        RotateMove();
    }

    private void OnMovement()
    {
        if (_combatSystem.IsAttacking) return;
        Vector2 inputMovement = _movement.ReadValue<Vector2>();

        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;
        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        _rawInputMovement = cameraForward * inputMovement.y + cameraRight * inputMovement.x;
        _isMove = _rawInputMovement.sqrMagnitude > 0.01f;

        anim.SetBool("running", _isMove);
    }

    private void RotateMove()
    {
        if (_rawInputMovement == Vector3.zero || _combatSystem.IsAttacking) return;
        
        Vector3 newPosition = transform.position + _rawInputMovement.normalized * (movementSpeed * Time.deltaTime);
        _rb.MovePosition(newPosition);

        Quaternion targetRotation = Quaternion.LookRotation(_rawInputMovement, Vector3.up);
        _rb.rotation = Quaternion.Slerp(_rb.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    private void OnJump()
    {
        if (_isJumping || _combatSystem.IsAttacking) return;

        _isJumping = true;
        anim.SetBool("jumping", true);
        _rb.velocity = new Vector3(0, jumpForce, 0);
        Physics.gravity = new Vector3(0, gravityNum, 0);
    }

    private void OnLightAttack()
    {
        if (_isJumping) return;

        _combatSystem.PerformAttack(AttackType.Light); 
    }
    
    private void OnHeavyAttack()
    {
        if (_isJumping) return;

        _combatSystem.PerformAttack(AttackType.Heavy); 
    }

    private void OnCollisionEnter(Collision other)
    {
        int collisionLayer = other.gameObject.layer;
        if (collisionLayer == LayerMask.NameToLayer("Ground"))
        {
            anim.SetBool("jumping", false);
            _isJumping = false;
        }
    }
}
