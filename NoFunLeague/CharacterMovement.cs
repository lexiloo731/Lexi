/*
 * Author: Alexia Nguyen
 * Description: Player is able to move in any direction in a 2D space. 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public static bool isRunning, isThrowing;
    public float moveSpeed = 4;
    public float raycastDistance = 0.1f;

    public Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveForce;
        Vector2 PlayerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        // Player walking and running speed
        if (isRunning == true)
        {
             moveForce = PlayerInput * moveSpeed * 2f;
        }
        else
        { 
            moveForce = PlayerInput * moveSpeed;
        }

        rb.velocity = moveForce;
        // Track speed for animator
        float speed = moveForce.magnitude;
        animator.SetFloat("Speed", speed);

        // Flip the sprite based on movement direction
        // Moving left
        if (moveForce.x > 0)
        {
            spriteRenderer.flipX = true;
        }
        // Moving right
        else if (moveForce.x < 0) 
        {
            spriteRenderer.flipX = false; 
        }
    }
}