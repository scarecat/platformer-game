using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Timeline;
using TMPro;
using Unity.VisualScripting;
using System;

public enum PlayerState
{
    Idle,
    Running,
    Jumping,
    Falling,
    Blocking
}

public class PlayerMovement : MonoBehaviour
{
    public TMP_Text stateText;

    [SerializeField] private int extraJumpCount = 1;
    private int currentExtraJumpCount;


    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpingPower = 16f;
    //private float fallLimit = -10f;
    private bool isFacingRight = true;
    private InputAction jumpAction;
    private InputAction moveAction;
    private PlayerState playerState;
    private InputAction blockAction;
    private Rigidbody2D rb;
    private Vector2 movementInput;
    private bool jumpedThisFrame = false;

    [SerializeField] private Transform groundCheck;
    //[SerializeField] private Transform respawnPoint;
    [SerializeField] private LayerMask groundLayer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpAction = InputSystem.actions.FindAction("Jump");
        moveAction = InputSystem.actions.FindAction("Move");
        blockAction = InputSystem.actions.FindAction("Block");

        jumpAction.performed += (ctx) =>
        {
            jumpedThisFrame = true;
        };


      currentExtraJumpCount = extraJumpCount;
    }

    void HandleJumping()
    {
        if (jumpedThisFrame && isGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
            playerState = PlayerState.Jumping;
        }
    }

    void HandleExtraJumping()
    {
        if (jumpedThisFrame && currentExtraJumpCount > 0)
        {
            currentExtraJumpCount--;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
            playerState = PlayerState.Jumping;
        }
    }


    void HandleRunning()
    {
        rb.linearVelocity = new Vector2(movementInput.x * speed, rb.linearVelocity.y);
    }


    void Update()
    {
        movementInput = moveAction.ReadValue<Vector2>();

        switch (playerState)
        {
            case PlayerState.Idle:
                currentExtraJumpCount = extraJumpCount;
                if (blockAction.IsPressed()) { playerState = PlayerState.Blocking; }
                else if (Mathf.Abs(movementInput.x) > 0.1f) { playerState = PlayerState.Running; }
                else if (!isGrounded())
                {
                    playerState = PlayerState.Falling;
                }
                HandleJumping();
                break;
            case PlayerState.Running:
                if (Mathf.Abs(movementInput.x) < 0.1f) { playerState = PlayerState.Idle; }
                HandleJumping();
                break;
            case PlayerState.Jumping:
                if (rb.linearVelocity.y <= 0) playerState = PlayerState.Falling;
                HandleExtraJumping();
                break;
            case PlayerState.Falling:
                if (isGrounded()) playerState = PlayerState.Idle;
                HandleExtraJumping();
                break;
            case PlayerState.Blocking:
                FaceMovementDir();
                break;
        }

        if (!isGrounded() && rb.linearVelocity.y < 0)
        {
            playerState = PlayerState.Falling;
        }

        FaceMovementDir();
        jumpedThisFrame = false;
    }


    private void FixedUpdate()
    {
        switch (playerState)
        {
            case PlayerState.Idle:
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
                break;
            case PlayerState.Running:
                HandleRunning();
                break;
            case PlayerState.Jumping:
                HandleRunning();
                break;
            case PlayerState.Falling:
                HandleRunning();
                break;
            case PlayerState.Blocking:
                break;
        }
    }

    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void FaceMovementDir()
    {
        if (isFacingRight && movementInput.x < 0f || !isFacingRight && movementInput.x > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
    public bool IsBlocking() => playerState == PlayerState.Blocking;
    public bool IsFacingRight() => isFacingRight;

}