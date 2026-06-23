using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Move Settings")]
    [SerializeField] private float moveSpeed = 5f;


    Rigidbody2D playerRb;
    PlayerInputActions playerControls;
    public Vector2 moveInput;

    private void Awake()
    {
        playerControls = new PlayerInputActions();
        playerRb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        playerControls.Enable();

        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = playerControls.Player.Move.ReadValue<Vector2>();


    }

    private void FixedUpdate()
    {
        playerRb.velocity = new Vector2(moveInput.x * moveSpeed, playerRb.velocity.y);
        
    }
    private void OnDisable()
    {
        playerControls.Disable();
    }
}
