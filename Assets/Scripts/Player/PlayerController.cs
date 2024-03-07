using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float maxSpeed = 7;
    public float timeToMaxSpeed = 0.1f;
    public float airMovementReduction = 0.7f;

    public float jumpHeight = 3;
    public float timeToPeak = 0.3f;
    public LayerMask ground;

    public Vector2 velocity = Vector2.zero;

    private float acceleration;
    private float jumpVelocity;
    private float gravity;

    private bool isGrounded;
    private bool canQueueJump;
    private bool jumping = false;
    private bool jumpQueued = false;
    private float jumpDelayDuration = 0.05f;

    private Rigidbody2D body;

    // Start is called before the first frame update
    void Start()
    {
        calculateConstants();
        body = GetComponent<Rigidbody2D>();
    }

    private void calculateConstants()
    {
        acceleration = maxSpeed / timeToMaxSpeed;
        jumpVelocity = (2 * jumpHeight) / timeToPeak;
        gravity = (-2 * jumpHeight) / (timeToPeak * timeToPeak);
    }

    private void Update()
    {
        calculateConstants();
        HorizontalMove();
        Jump();
    }

    private void FixedUpdate()
    {
        canQueueJump = CanQueueJump();
        isGrounded = IsGrounded();
        Gravity();
        body.velocity = velocity;
    }

    private void HorizontalMove()
    {
        float tempAcceleration = isGrounded ? acceleration : acceleration*airMovementReduction;
        float[] tempVelocity = { velocity.x, velocity.y };

        if (Input.GetKey(KeyCode.RightArrow))
        {
            tempVelocity[0] += tempAcceleration * Time.deltaTime;
            if (tempVelocity[0] > maxSpeed)
                tempVelocity[0] = maxSpeed;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            tempVelocity[0] -= tempAcceleration * Time.deltaTime;
            if (tempVelocity[0] < -maxSpeed)
                tempVelocity[0] = -maxSpeed;
        }

        if(!Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
        {
            if(tempVelocity[0] > 0)
            {
                tempVelocity[0] -= tempAcceleration * Time.deltaTime;
                if(tempVelocity[0] < 0)
                    tempVelocity[0] = 0;
            }else if(tempVelocity[0] < 0)
            {
                tempVelocity[0] += tempAcceleration * Time.deltaTime;
                if (tempVelocity[0] > 0)
                    tempVelocity[0] = 0;
            }
        }

        velocity = new Vector2(tempVelocity[0], tempVelocity[1]);
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) || jumpQueued)
        {
            if (isGrounded && !jumping)
            {
                velocity = new Vector2(velocity.x, jumpVelocity);
                jumpQueued = false;
                StartCoroutine(JumpDelay());
            } else if (!jumpQueued && canQueueJump)
            {
                jumpQueued = true;
            }
        }
        else if(Input.GetKeyUp(KeyCode.Space) && velocity.y > 0)
        {
            velocity = new Vector2(velocity.x, 0);
        }
        
    }

    private void Gravity()
    {
        float yVelocity = velocity.y;
        if (!isGrounded)
        {
            yVelocity += gravity*Time.deltaTime;
            if (yVelocity < -jumpVelocity)
                yVelocity = -jumpVelocity;
            velocity = new Vector2(velocity.x, yVelocity);
        }
        else if(!jumping)
        {
            velocity = new Vector2(velocity.x, 0);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(transform.position, new Vector2(1,1), 0, -Vector2.up, 0.05f, ground);
    }

    private bool CanQueueJump()
    {
        return Physics2D.BoxCast(transform.position, new Vector2(1, 1), 0, -Vector2.up, 1f, ground);
    }

    IEnumerator JumpDelay()
    {
        jumping = true;
        yield return new WaitForSeconds(jumpDelayDuration);
        jumping = false;
    }
}
