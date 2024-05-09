/*
 * Author: Alexia Nguyen
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;

    //controls
    public KeyCode left;
    public KeyCode right;
    public KeyCode jump;
    
    private Rigidbody2D theRB;

    //ground references
    public Transform groundCheckPoint;
    public bool isGrounded;
    public float groundCheckRadius;
    public LayerMask whatIsGround;
 
    void Start()
    {
        theRB = GetComponent<Rigidbody2D>();
    }

    // Used to get input from player
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, whatIsGround);

        // Checks for jump input
        if(Input.GetKeyDown(jump) && isGrounded)
        {
            theRB.velocity = new Vector2(theRB.velocity.x, jumpForce);
        }

        //makes character go left or right
        if(Input.GetKey(left))
        {
            theRB.velocity = new Vector2(-moveSpeed, theRB.velocity.y);
        }
        else if(Input.GetKey(right))
        {
            theRB.velocity = new Vector2(moveSpeed, theRB.velocity.y);
        }
        else
        {
            theRB.velocity = new Vector2(0, theRB.velocity.y);
        }

        //flip character if moving
        //in opposite direction
        if(theRB.velocity.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 0);
        }
        else if(theRB.velocity.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 0);
        }
    }
}
