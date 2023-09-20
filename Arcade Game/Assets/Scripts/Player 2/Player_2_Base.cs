using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Player_2_Base : MonoBehaviour
{
    // Player Config
    bool onGround = false;

    [SerializeField]
    float movementSpeed = 5.0f,
        dashSpeed = 10.0f,
        jumpSpeed = 10.0f,
        downSpeed = 50.0f;

    int jumpCounter = 0;
    const int jumpMax = 2;

    Rigidbody2D rb;
    
    int direction_multiplier = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Vertical Movement

        if (Input.GetKeyDown(KeyCode.UpArrow) && jumpCounter < jumpMax)
        {
            if (jumpCounter == 0) // First jump
            {
                jumpCounter++;
                rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
                onGround = false;
            }
            else if (jumpCounter > 0) // Second jump
            {
                jumpCounter++;
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.velocity = new Vector2(rb.velocity.x, jumpSpeed * 0.75f);
            }
        }

        if (onGround == false && Input.GetKeyDown(KeyCode.DownArrow)) // Boost down
        {
            rb.AddForce(new Vector3(0, -1 * 50, 0), ForceMode2D.Impulse);
        }

        // Dash

        if (Input.GetKey(KeyCode.Keypad1) && onGround)
        {
            rb.velocity = new Vector2(dashSpeed * direction_multiplier, rb.velocity.y);
        }

        // Horizontal Movement

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            direction_multiplier = -1;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            direction_multiplier = 1;
        }

        if (onGround && Math.Abs(rb.velocity.x) <= movementSpeed)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                rb.velocity = new Vector2(-movementSpeed, rb.velocity.y);
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                rb.velocity = new Vector2(movementSpeed, rb.velocity.y);
            }
        }
        else if ((onGround && Math.Abs(rb.velocity.x) > movementSpeed) || (onGround == false))
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                if (rb.velocity.x > 0)
                {
                    rb.velocity = new Vector2(rb.velocity.x - movementSpeed, rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(-Math.Abs(rb.velocity.x), rb.velocity.y);
                }
                
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                if (rb.velocity.x < 0)
                {
                    rb.velocity = new Vector2(rb.velocity.x + movementSpeed, rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(Math.Abs(rb.velocity.x), rb.velocity.y);
                }

            }
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Stage")
        {
            onGround = true;
            jumpCounter = 0;
        }
    }
}