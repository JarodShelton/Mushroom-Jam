using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float maxSpeed = 10;
    [SerializeField] float timeToMaxSpeed = 0.1f;
    [SerializeField] float airMovementReduction = 0.7f;

    [SerializeField] float jumpHeight = 3;
    [SerializeField] float timeToPeak = 0.3f;

    [SerializeField] float wallJumpHeight = 3;
    [SerializeField] float wallJumpSpeed = 10;
    [SerializeField] float wallJumpReduction = 0.3f;
    [SerializeField] float wallJumpReductionTime = 0.1f;
    [SerializeField] float wallSlideSpeed = 7;
    [SerializeField] float timeToMaxSlideSpeed = 0.1f;

    [SerializeField] float pogoHeight = 1;

    [SerializeField] float blastLength = 7;
    [SerializeField] float blastDuration = 0.2f;

    [SerializeField] LayerMask ground;
    [SerializeField] LayerMask wall;
    [SerializeField] LayerMask ceiling;

    public Vector2 velocity = Vector2.zero;

    private Direction walkingDirection;

    private float acceleration;
    private float jumpVelocity;
    private float pogoVelocity;
    private float gravity;
    private float blastVelocity;
    private float wallSlideGravity;
    private float wallJumpVelocity;

    private bool isGrounded;
    private bool canQueueJump;
    private bool jumping = false;
    private bool wallJumping = false;
    private bool jumpQueued = false;
    private bool canFastfall = false;
    private bool blasting = false;
    private bool walledLeft = false;
    private bool walledRight = false;

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
        wallSlideGravity = -wallSlideSpeed / timeToMaxSlideSpeed;
        wallJumpVelocity = Mathf.Sqrt(-2 * gravity * wallJumpHeight);
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
        walledLeft = IsWalled(Direction.Left);
        walledRight = IsWalled(Direction.Right);

        if(!blasting)
            Gravity();

        bool touchingCeiling = Physics2D.BoxCast(transform.position, new Vector2(1, 1), 0, Vector2.up, 0.05f, ceiling);
        if (touchingCeiling)
        {
            velocity = new Vector2(velocity.x, Mathf.Clamp(velocity.y, -jumpVelocity, 0));
            blasting = false;
        }

        body.velocity = velocity;
    }

    private void HorizontalMove()
    {
        float tempAcceleration = acceleration;

        if (wallJumping)
            tempAcceleration *= wallJumpReduction;
        else if (!isGrounded)
            tempAcceleration *= airMovementReduction;

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
            }
            else if(tempVelocity < 0)
            {
                tempVelocity += tempAcceleration * Time.deltaTime;
                if (tempVelocity > 0)
                    tempVelocity = 0;
            }
        }

        if(walledLeft && !wallJumping)
            tempVelocity = Mathf.Clamp(tempVelocity, 0, maxSpeed);
        else if(walledRight && !wallJumping)
            tempVelocity = Mathf.Clamp(tempVelocity, -maxSpeed, 0);
        else
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
            } 
            else if (!jumpQueued && canQueueJump)
            {
                jumpQueued = true;
            } 
            else if(Walled() && !jumping && !isGrounded)
            {
                jumpQueued = false;
                float speed = walledLeft ? wallJumpSpeed : -wallJumpSpeed;
                velocity = new Vector2(speed, wallJumpVelocity);
                StartCoroutine(JumpDelay());
                StartCoroutine(WallJumpDelay());
                canFastfall = false;
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
            if(HuggingWall() && yVelocity < 0)
            {
                yVelocity += wallSlideGravity * Time.deltaTime;
                yVelocity = Mathf.Clamp(yVelocity, -wallSlideSpeed, 0);
                velocity = new Vector2(velocity.x, yVelocity);
            }
            else
            {
                yVelocity += gravity * Time.deltaTime;
                yVelocity = Mathf.Clamp(yVelocity, -jumpVelocity, 1000);
                velocity = new Vector2(velocity.x, yVelocity);
            }
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

    public bool Walled()
    {
        return walledLeft || walledRight;
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(transform.position, new Vector2(1,1), 0, Vector2.down, 0.05f, ground);
    }

    private bool IsWalled(Direction direction)
    {
        if (direction == Direction.Right)
            return Physics2D.BoxCast(transform.position, new Vector2(1, 1), 0, Vector2.right, 0.05f, wall);
        else
            return Physics2D.BoxCast(transform.position, new Vector2(1, 1), 0, Vector2.left, 0.05f, wall);
    }

    private bool CanQueueJump()
    {
        return Physics2D.BoxCast(transform.position, new Vector2(1, 1), 0, -Vector2.up, 1f, ground);
    }

    public bool HuggingWall()
    {
        return walledLeft && Input.GetKey(KeyCode.LeftArrow) || walledRight && Input.GetKey(KeyCode.RightArrow);
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
        velocity = new Vector2(Mathf.Clamp(velocity.x, -maxSpeed, maxSpeed), Mathf.Clamp(velocity.y, -jumpVelocity, jumpVelocity));
        blasting = false;
    }

    IEnumerator WallJumpDelay()
    {
        wallJumping = true;
        yield return new WaitForSeconds(wallJumpReductionTime);
        wallJumping = false;

    }
}
