using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float maxSpeed = 7;
    [SerializeField] float timeToMaxSpeed = 0.1f;
    [SerializeField] float airMovementReduction = 0.7f;

    [SerializeField] float jumpHeight = 3;
    [SerializeField] float timeToPeak = 0.3f;
    [SerializeField] float pogoHeight = 1;

    [SerializeField] LayerMask ground;

    public Vector2 velocity = Vector2.zero;

    private Direction walkingDirection;

    private float acceleration;
    private float jumpVelocity;
    private float pogoVelocity;
    private float gravity;

    private bool isGrounded;
    private bool canQueueJump;
    private bool jumping = false;
    private bool jumpQueued = false;
    private bool canFastfall = false;

    private float jumpDelayDuration = 0.05f;

    private Rigidbody2D body;

    public enum Direction { Left, Right, Up , Down}

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
        pogoVelocity = Mathf.Sqrt(-2 * gravity * pogoHeight);
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
            walkingDirection = Direction.Right;
            tempVelocity[0] += tempAcceleration * Time.deltaTime;
            if (tempVelocity[0] > maxSpeed)
                tempVelocity[0] = maxSpeed;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            walkingDirection = Direction.Left;
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
                canFastfall = true;
                StartCoroutine(JumpDelay());
            } else if (!jumpQueued && canQueueJump)
            {
                jumpQueued = true;
            }
        }
        else if(canFastfall && Input.GetKeyUp(KeyCode.Space) && velocity.y > 0)
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

    public void Pogo()
    {
        velocity = new Vector2(velocity.x, pogoVelocity);
        canFastfall = false;
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(transform.position, new Vector2(1,1), 0, -Vector2.up, 0.05f, ground);
    }

    private bool CanQueueJump()
    {
        return Physics2D.BoxCast(transform.position, new Vector2(1, 1), 0, -Vector2.up, 1f, ground);
    }

    public Direction GetFacingDirection()
    {
        if (Input.GetKey(KeyCode.DownArrow))
            return Direction.Down;

        if (Input.GetKey(KeyCode.UpArrow))
            return Direction.Up;

        if (Input.GetKey(KeyCode.RightArrow))
            return Direction.Right;

        if (Input.GetKey(KeyCode.LeftArrow))
            return Direction.Left;

        return walkingDirection;
    }

    IEnumerator JumpDelay()
    {
        jumping = true;
        yield return new WaitForSeconds(jumpDelayDuration);
        jumping = false;
    }
}
