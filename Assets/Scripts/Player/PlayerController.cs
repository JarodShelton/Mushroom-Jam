using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float maxSpeed = 3;
    public float timeToMaxSpeed = 0.1f;
    public Vector2 velocity = Vector2.zero;

    private float acceleration;
    private Rigidbody2D body;
    // Start is called before the first frame update
    void Start()
    {
        acceleration = maxSpeed / timeToMaxSpeed;
        body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        acceleration = maxSpeed / timeToMaxSpeed;
        GroundHorizontalMove();
        body.velocity = velocity;
    }

    private void GroundHorizontalMove()
    {
        float[] tempVelocity = { velocity.x, velocity.y };

        if (Input.GetKey(KeyCode.RightArrow))
        {
            Debug.Log("Moving Right!");
            tempVelocity[0] += acceleration * Time.deltaTime;
            if (tempVelocity[0] > maxSpeed)
                tempVelocity[0] = maxSpeed;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Debug.Log("Moving Left!");
            tempVelocity[0] -= acceleration * Time.deltaTime;
            if (tempVelocity[0] < -maxSpeed)
                tempVelocity[0] = -maxSpeed;
        }

        if(!Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
        {
            if(tempVelocity[0] > 0)
            {
                tempVelocity[0] -= acceleration * Time.deltaTime;
                if(tempVelocity[0] < 0)
                    tempVelocity[0] = 0;
            }else if(tempVelocity[0] < 0)
            {
                tempVelocity[0] += acceleration * Time.deltaTime;
                if (tempVelocity[0] > 0)
                    tempVelocity[0] = 0;
            }
        }

        velocity = new Vector2(tempVelocity[0], tempVelocity[1]);
    }
}
