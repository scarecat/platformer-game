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

    private int _extraJumpCount = 1;
    public int ExtraJumpCount
    {
        set { _extraJumpCount = value; }
        get => _extraJumpCount;
    }



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
    private PlayerEnergy playerEnergy;

    [SerializeField] private Coroutine speedCoroutine;
    [SerializeField] private Coroutine jumpCoroutine;

    [SerializeField] private Transform groundCheck;
    //[SerializeField] private Transform respawnPoint;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float knockbackForce = 10.0f;

    public bool IsBlocking() => playerState == PlayerState.Blocking;
    public bool IsFacingRight() => isFacingRight;

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

        currentExtraJumpCount = ExtraJumpCount;

        animator = GetComponent<Animator>();
        jumpAction = InputSystem.actions.FindAction("Jump");
        moveAction = InputSystem.actions.FindAction("Move");
        blockAction = InputSystem.actions.FindAction("Block");
        playerEnergy = GetComponent<PlayerEnergy>();
    }

    private void StopVelocity()
    {
        rb.linearVelocity = Vector2.zero;
        movementInput = Vector2.zero;
    }

    private IEnumerator Attack()
    {
        StopVelocity();
        playerState = PlayerState.Attacking;
        movementInput = Vector2.zero;
        animator.SetTrigger("attack");
        yield return new WaitForSeconds(1.0f / 12);
        playerHitbox.SetActive(true);
        yield return new WaitForSeconds(2.0f / 12);
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

    void HandleBlocking()
    {
        playerEnergy.UseBlockEnergy();
        if (!blockAction.IsPressed()) { playerState = PlayerState.Idle; }
    }


    bool HandleJumping()
    {
        if (jumpedThisFrame && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
            playerState = PlayerState.Jumping;
            animator.SetTrigger("jump");
            return true;
        }
        return false;
    }

    bool TryExtraJump()
    {
        if (jumpedThisFrame && currentExtraJumpCount > 0)
        {
            currentExtraJumpCount--;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
            playerState = PlayerState.Jumping;
            animator.ResetTrigger("jump");
            return true;
        }
        return false;
    }


    void HandleRunning()
    {
        rb.linearVelocity = new Vector2(movementInput.x * speed, rb.linearVelocity.y);
    }

    void Update()
    {
        animator.SetBool("isMoving", playerState == PlayerState.Running);
        animator.SetBool("isFalling", playerState == PlayerState.Falling);
        animator.SetBool("isBlocking", playerState == PlayerState.Blocking);
        animator.SetBool("isGroundState", playerState == PlayerState.Idle || playerState == PlayerState.Running);

        movementInput = moveAction.ReadValue<Vector2>();


        switch (playerState)
        {
            case PlayerState.Idle:
                currentExtraJumpCount = ExtraJumpCount;
                if (TryBlock()) break;
                if (Mathf.Abs(movementInput.x) > 0.1f) { playerState = PlayerState.Running; break; }
                else if (!IsGrounded()) { playerState = PlayerState.Falling; break; }
                if (HandleAttack()) break;
                if (HandleJumping()) break;
                break;
            case PlayerState.Running:
                if (TryBlock()) break;
                if (Mathf.Abs(movementInput.x) == 0) { playerState = PlayerState.Idle; break; }
                if (HandleAttack()) break;
                if (HandleJumping()) break;
                break;
            case PlayerState.Jumping:
                if (rb.linearVelocity.y <= 0) playerState = PlayerState.Falling;
                if (TryExtraJump()) break;
                break;
            case PlayerState.Falling:
                if (IsGrounded()) playerState = PlayerState.Idle;
                if (TryExtraJump()) break;
                break;
            case PlayerState.Blocking:
                FaceMovementDir();
                rb.linearVelocityX = 0.0f;
                break;
        }

        if (!IsGrounded() && rb.linearVelocity.y < -0.1f)
        {
            playerState = PlayerState.Falling;
        }

        FaceMovementDir();
        jumpedThisFrame = false;

    }

    private bool TryBlock()
    {
        if (blockAction.IsPressed())
        {
            StopVelocity();
            playerState = PlayerState.Blocking;
            return true;
        }
        return false;
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
                HandleBlocking();
                break;
        }
    }

    private bool IsGrounded()
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

    public void Knockback(Vector2 damageDirection)
    {
        rb.AddForce(damageDirection * knockbackForce);
    }

    public void ApplySpeedBoost(float multiplier, float duration)
    {
        if(speedCoroutine != null)
        {
            StopCoroutine(speedCoroutine);
        }

        speedCoroutine = StartCoroutine(SpeedBoost(multiplier, duration));
    }

    private IEnumerator SpeedBoost(float multiplier, float duration)
    {
        float originalSpeed = speed;
        speed *= multiplier;

        yield return new WaitForSeconds(duration);

        speed = originalSpeed;
    }

    public void ApplyExtraJump(int extraJumps, float duration)
    {
        if (jumpCoroutine != null)
        {
            StopCoroutine(jumpCoroutine);
        }

        jumpCoroutine = StartCoroutine(ExtraJump(extraJumps, duration));
    }

    private IEnumerator ExtraJump(int extraJumps, float duration)
    {
        int originalExtraJumps = _extraJumpCount;
        ExtraJumpCount = originalExtraJumps + extraJumps;

        yield return new WaitForSeconds(duration);

        ExtraJumpCount = originalExtraJumps;
    }
}
