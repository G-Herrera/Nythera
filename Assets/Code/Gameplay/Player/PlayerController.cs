using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Move Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Transform graphics;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [HideInInspector] public Vector2 moveInput;
    public Vector2 LookInput => lookInput;

    Rigidbody2D playerRb;
    PlayerInputActions playerControls;
    PlayerAnimation playerAnimation;

    private Vector2 lookInput;
    private bool isGrounded;
    private bool jumpPressed;

    //Propierties
    public bool IsFacingRight { get; private set; } = false;

    private void Awake()
    {
        playerControls = new PlayerInputActions();
        playerRb = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponent<PlayerAnimation>();
    }

    private void OnEnable()
    {
        playerControls.Enable();
        playerControls.Player.Jump.performed += Jump;


    }
    private void OnDisable()
    {
        playerControls.Disable();
        playerControls.Player.Jump.performed -= Jump;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
        moveInput = playerControls.Player.Move.ReadValue<Vector2>();

        lookInput = playerControls.Player.Look.ReadValue<Vector2>();

        playerAnimation.UpdateMovement(Mathf.Abs(moveInput.x));

        playerAnimation.UpdateGrounded(isGrounded);

        jumpPressed = playerControls.Player.Jump.IsPressed();

    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 
                                             groundRadius, groundLayer);

        playerRb.velocity = new Vector2(moveInput.x * moveSpeed, playerRb.velocity.y);

        BetterJump();

        Flip();

    }

    private void BetterJump()
    {
        if (playerRb.velocity.y < 0)
        {
            playerRb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (playerRb.velocity.y > 0 && !jumpPressed)
        {
            playerRb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }
    private void Jump(InputAction.CallbackContext context)
    {

        Jump();
        
    }

    private void Jump()
    {
        if (!isGrounded) return;
        playerRb.velocity = new Vector2(playerRb.velocity.x, jumpForce);
    }

    private void Flip()
    {
        if (moveInput.x > 0 && !IsFacingRight)
        {
            SetFacingDirection(true);

        }
        else if (moveInput.x < 0 && IsFacingRight)
        {
            SetFacingDirection(false);
        }
    }

    private void SetFacingDirection(bool facingRight)
    {
        IsFacingRight = facingRight;

        Vector3 scale = graphics.localScale;

        scale.x = facingRight ? -1f : 1f;

        graphics.localScale = scale;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }
}
