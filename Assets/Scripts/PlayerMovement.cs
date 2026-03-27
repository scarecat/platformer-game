using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Timeline;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public TMP_Text stateText;
    
    private float horizontal;
    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpingPower = 16f;
    //private float fallLimit = -10f;
    private bool isFacingRight = true;
    private InputAction jumpAction;
    private InputAction moveAction;
    private PlayerState currentState;
    private bool isBlocking = false;
    private InputAction blockAction;
    private Rigidbody2D rb;

    [SerializeField] private Transform groundCheck;
    //[SerializeField] private Transform respawnPoint;
    [SerializeField] private LayerMask groundLayer;

    private void Start()
    {
      rb = GetComponent<Rigidbody2D>();
      jumpAction = InputSystem.actions.FindAction("Jump");
      moveAction = InputSystem.actions.FindAction("Move");
      blockAction = InputSystem.actions.FindAction("Block");
    }

    void Update()
    {
        horizontal = moveAction.ReadValue<Vector2>().x;

        isBlocking = blockAction.IsPressed();

        if (isBlocking)
        {
            horizontal = 0f;
            currentState = PlayerState.Blocking;
            if (stateText != null)
                stateText.text = "Player: " + currentState.ToString();
            Flip(); 
            return; 
        }
        if(jumpAction.IsPressed() && isGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
        }

        if(!isGrounded())
        {
            if(rb.linearVelocity.y > 0)
                currentState = PlayerState.Jumping;
            else
                currentState = PlayerState.Falling;
        }
        else if(Mathf.Abs(horizontal) > 0.1f)
        {
            currentState = PlayerState.Running;
        }
        else
        {
            currentState = PlayerState.Idle;
        }

        if(stateText != null)
            stateText.text = "Player: " + currentState.ToString();
        
        Flip();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
    }

    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if(isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
    public bool IsBlocking() => isBlocking;
    public bool IsFacingRight() => isFacingRight;
}
