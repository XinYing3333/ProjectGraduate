using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerCtrl : MonoBehaviour
{
    [Header("Input Settings")]
    public PlayerInput playerInput;
    [HideInInspector]public Animator anim;
    public Transform cameraTransform;
    private Rigidbody _rb;

    [SerializeField] private float movementSpeed = 10.0f;
    [SerializeField] private float turnSpeed = 20f;
    [SerializeField] private float gravityNum = 30f;
    
    private InputAction _movement;
    private Vector3 _rawInputMovement;
    private bool _isMove;
    
    private InputAction _jump;
    private bool _isJumping;
    private bool _isGrounded;
    [SerializeField] private float jumpForce = 10f;

    [HideInInspector]public bool isAttacking;
    private CombatSystem _combatSystem;


    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        _combatSystem = GetComponent<CombatSystem>();
        
        _movement = playerInput.actions.FindAction("Movement");
        _jump = playerInput.actions.FindAction("Jump");
        _jump.performed += ctx => OnJump();
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        OnMovement();

        if (Input.GetMouseButtonDown(0) && !_isJumping && !isAttacking)
        {
            anim.SetTrigger("attacking");
            isAttacking = true;
            _combatSystem.PerformAttack();

            StartCoroutine(ResetAttackState());
        }
    }


    void FixedUpdate()
    {
        RotateMove();
    }
    
    private void OnMovement()
    {
        if (isAttacking) return;
        
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
        if (_rawInputMovement == Vector3.zero || isAttacking) return;

        Vector3 newPosition = transform.position + _rawInputMovement.normalized * (movementSpeed * Time.deltaTime);
        _rb.MovePosition(newPosition);

        Quaternion targetRotation = Quaternion.LookRotation(_rawInputMovement, Vector3.up);
        _rb.rotation = Quaternion.Slerp(_rb.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    private void OnJump()
    {
        if (_isJumping || isAttacking) return;
        
        _isJumping = true;
        anim.SetBool("jumping", true);
        _rb.velocity = new Vector3(0, jumpForce, 0);
        Physics.gravity = new Vector3(0, gravityNum, 0);
    }
    
    private IEnumerator ResetAttackState()
    {
        yield return new WaitForSeconds(0.6f);
        isAttacking = false; 
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
