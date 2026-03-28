using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Timeline;
using TMPro;
using Unity.VisualScripting;
using System;
using System.Collections;
using UnityEngine.Playables;

public enum PlayerState
{
    Idle,
    Running,
    Jumping,
    Falling,
    Blocking,
    Attacking
}

public class PlayerMovement : MonoBehaviour
{
    public TMP_Text stateText;

    [SerializeField] private int extraJumpCount = 1;
    private int currentExtraJumpCount;

    private GameObject playerHitbox;

    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpingPower = 16f;
    //private float fallLimit = -10f;
    private bool isFacingRight = true;
    private InputAction jumpAction;
    private InputAction moveAction;
    private InputAction blockAction;
    private InputAction attackAction;
    private Animator animator;
    private Rigidbody2D rb;
    private Vector2 movementInput;
    public PlayerState playerState;
    private bool jumpedThisFrame = false;

    [SerializeField] private Transform groundCheck;
    //[SerializeField] private Transform respawnPoint;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float knockbackForce = 10.0f;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpAction = InputSystem.actions.FindAction("Jump");
        moveAction = InputSystem.actions.FindAction("Move");
        blockAction = InputSystem.actions.FindAction("Block");
        attackAction = InputSystem.actions.FindAction("Attack");

        jumpAction.performed += (ctx) => jumpedThisFrame = true;

        playerHitbox = GameObject.Find("Hitbox");
        playerHitbox.SetActive(false);

        currentExtraJumpCount = extraJumpCount;

        animator = GetComponent<Animator>();
    }

    private IEnumerator Attack()
    {
        playerState = PlayerState.Attacking;
        animator.SetTrigger("attack");
        yield return new WaitForSeconds(1.0f/12);
        playerHitbox.SetActive(true);
        yield return new WaitForSeconds(2.0f/12);
        playerHitbox.SetActive(false);
        playerState = PlayerState.Idle;
    }


    
    bool HandleAttack()
    {
        if (attackAction.IsPressed())
        {
            StartCoroutine(Attack());
            return true;
        }
        return false;
    }


    bool HandleJumping()
    {
        if (jumpedThisFrame && isGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
            playerState = PlayerState.Jumping;
            animator.SetTrigger("jump");
            return true;
        }
        return false;
    }

    void HandleExtraJumping()
    {
        if (jumpedThisFrame && currentExtraJumpCount > 0)
        {
            currentExtraJumpCount--;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
            playerState = PlayerState.Jumping;
            animator.ResetTrigger("jump");
        }
    }


    void HandleRunning()
    {
        rb.linearVelocity = new Vector2(movementInput.x * speed, rb.linearVelocity.y);
    }




    void Update()
    {
        animator.SetBool("isMoving", playerState == PlayerState.Running);
        animator.SetBool("isFalling", playerState == PlayerState.Falling);

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
                if (HandleAttack()) break;
                if (HandleJumping()) break;
                break;
            case PlayerState.Running:
                if (Mathf.Abs(movementInput.x) == 0) { playerState = PlayerState.Idle; }
                if (HandleAttack()) break;
                if (HandleJumping()) break;
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

        if (!isGrounded() && rb.linearVelocity.y < -0.1f)
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

    public void Knockback(Vector2 damageDirection)
    {
        rb.AddForce(damageDirection * knockbackForce);
    }
}