using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Timeline;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 16f;
    //private float fallLimit = -10f;
    private bool isFacingRight = true;
    private InputAction jumpAction;
    private InputAction moveAction;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    //[SerializeField] private Transform respawnPoint;
    [SerializeField] private LayerMask groundLayer;

    private void Start()
    {
       jumpAction = InputSystem.actions.FindAction("Jump");
       moveAction = InputSystem.actions.FindAction("Move");
    }

    void Update()
    {
        horizontal = moveAction.ReadValue<Vector2>().x;

        if(jumpAction.IsPressed() && isGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
        }

        //if(transform.position.y < fallLimit)
        //{
        //    transform.position = respawnPoint.position;
        //}

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
}
