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

    [SerializeField] float blastLength = 7;
    [SerializeField] float blastDuration = 0.2f;

    [SerializeField] LayerMask ground;

    public Vector2 velocity = Vector2.zero;

    private Direction walkingDirection;

    private float acceleration;
    private float jumpVelocity;
    private float pogoVelocity;
    private float gravity;
    private float blastVelocity;

    private bool isGrounded;
    private bool canQueueJump;
    private bool jumping = false;
    private bool jumpQueued = false;
    private bool canFastfall = false;
    private bool blasting = false;

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
        blastVelocity = blastLength / blastDuration;
    }

    private void Update()
    {
        calculateConstants();
        if (!blasting)
        {
            HorizontalMove();
            Jump();
        }
    }

    private void FixedUpdate()
    {
        canQueueJump = CanQueueJump();
        isGrounded = IsGrounded();
        if(!blasting)
            Gravity();
        body.velocity = velocity;
    }

    private void HorizontalMove()
    {
        float tempAcceleration = isGrounded ? acceleration : acceleration*airMovementReduction;
        float tempVelocity = velocity.x;

        if (Input.GetKey(KeyCode.RightArrow))
        {
            walkingDirection = Direction.Right;
            tempVelocity += tempAcceleration * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            walkingDirection = Direction.Left;
            tempVelocity -= tempAcceleration * Time.deltaTime;
            
        }

        if(!Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
        {
            if(tempVelocity > 0)
            {
                tempVelocity -= tempAcceleration * Time.deltaTime;
                if(tempVelocity < 0)
                    tempVelocity = 0;
            }else if(tempVelocity < 0)
            {
                tempVelocity += tempAcceleration * Time.deltaTime;
                if (tempVelocity > 0)
                    tempVelocity = 0;
            }
        }

        tempVelocity = Mathf.Clamp(tempVelocity, -maxSpeed, maxSpeed);
        velocity = new Vector2(tempVelocity, velocity.y);
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.C) || jumpQueued)
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
        else if(canFastfall && Input.GetKeyUp(KeyCode.C) && velocity.y > 0)
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
            yVelocity = Mathf.Clamp(yVelocity, -jumpVelocity, jumpVelocity);
            velocity = new Vector2(velocity.x, yVelocity);
        }
        else if(!jumping)
        {
            velocity = new Vector2(velocity.x, 0);
        }
    }

    public void Blast(Vector2 direction)
    {
        velocity = direction.normalized * blastVelocity;
        canFastfall = false;
        StartCoroutine(BlastDelay());
    }

    public void Pogo()
    {
        if (!isGrounded)
        {
            velocity = new Vector2(velocity.x, pogoVelocity);
            canFastfall = false;
        }
        
    }

    public bool Grounded()
    {
        return isGrounded;
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

    IEnumerator BlastDelay()
    {
        blasting = true;
        yield return new WaitForSeconds(blastDuration);
        blasting = false;
    }
}
