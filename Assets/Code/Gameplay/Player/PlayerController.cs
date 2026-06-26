using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Move Settings")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    public Vector2 moveInput;

    Rigidbody2D playerRb;
    PlayerInputActions playerControls;

    private bool isGrounded;
    private bool jumpPressed;

    private void Awake()
    {
        playerControls = new PlayerInputActions();
        playerRb = GetComponent<Rigidbody2D>();
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

        jumpPressed = playerControls.Player.Jump.IsPressed();

    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 
                                             groundRadius, groundLayer);

        playerRb.velocity = new Vector2(moveInput.x * moveSpeed, playerRb.velocity.y);

        BetterJump();

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

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }
}
