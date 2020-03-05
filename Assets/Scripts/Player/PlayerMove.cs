using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState 
{
    Idle, Running, Landing, Jumping
}

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 3f;
    [SerializeField] private float groundCheckDistance = 0.1f;

    [SerializeField] private float maxVelocity = 10f;

    private Rigidbody2D playerRb;
    private PolygonCollider2D playerCollider;
    private PlayerState currentState;
    private RaycastHit2D[] raycastHit2D = new RaycastHit2D[16];

    // Start is called before the first frame update
    void Start()
    {
        playerCollider = GetComponent<PolygonCollider2D>();
        currentState = PlayerState.Idle;
        playerRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case PlayerState.Running:
            case PlayerState.Idle:
                Jump();
                break;
            case PlayerState.Jumping:
                CheckIfGrounded();
                break;
            case PlayerState.Landing:
                currentState = PlayerState.Idle;
                break;
        }        
    }

    private void CheckIfGrounded()
    {
        int numberOfRaycastHits = playerCollider.Cast(Vector2.down, raycastHit2D, groundCheckDistance);
        if (numberOfRaycastHits > 0)
        {
            currentState = PlayerState.Landing;
        }        
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            currentState = PlayerState.Jumping;
        }
        
        switch (currentState)
        {
            case PlayerState.Running:
                // Normal, forward rolling jump
                break;
            case PlayerState.Idle:
                // Idle Jumping
                break;
        }
    }

    private void FixedUpdate()
    {
        float xAxis = Input.GetAxis("Horizontal");
        playerRb.AddForce(Vector2.right * moveSpeed * xAxis, ForceMode2D.Force);
        playerRb.velocity = Vector2.ClampMagnitude(playerRb.velocity.x, maxVelocity);
    }
}
