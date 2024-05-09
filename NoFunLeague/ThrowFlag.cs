/*
 * Author: Alexia Nguyen
 * Description: Player is able to draw a trajectory until mouse button is released. Once mouse button is released, flag object spawns into scene and trajectroy line dissapears.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowFlag : MonoBehaviour
{
    [SerializeField] Transform projectilePrefab;
    [SerializeField] Transform spawnPoint;
    [SerializeField] LineRenderer lineRenderer;

    [SerializeField] public float launchForce;
    [SerializeField] float trajectoryTimeStep = 0.05f;
    [SerializeField] int trajectoryStepCount = 15;

    // [SerializeField] AudioSoure thrownFlagSFX;

    Vector2 velocity, startMousePos, currentMousePos;

    public Animator animator;
    public SpriteRenderer spriteRenderer;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        // On left click, show trajectory 
        if (Input.GetMouseButton(0))
        {
            currentMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            velocity = (startMousePos - currentMousePos) * launchForce;

            DrawTrajectory();
        }

        // Throw flag and then clear trajectory line
        if (Input.GetMouseButtonUp(0))
        {
            animator.SetBool("isThrowing", false);
            //thrownFlagSFX.play();
            animator.SetTrigger("hasThrownTrigger");
            FireProjectile();
            clearTrajectory();
        }
    }

    // Use line renderer to show player where flag is going
    void DrawTrajectory()
    {
        animator.SetBool("isThrowing", true);
        Vector3[] positions = new Vector3[trajectoryStepCount];
        for (int i = 0; i < trajectoryStepCount; i++)
        {
            float t = i * trajectoryTimeStep;
            Vector3 pos = (Vector2)spawnPoint.position + velocity * t + 0.5f * Physics2D.gravity * t * t;

            positions[i] = pos;
        }

        lineRenderer.positionCount = trajectoryStepCount;
        lineRenderer.SetPositions(positions);
    }

    // Throw flag
    void FireProjectile()
    {
        Transform pr = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
        pr.GetComponent<Rigidbody2D>().velocity = velocity;
    }

    // Clear trajectory line in line renderer
    void clearTrajectory()
    {
        lineRenderer.positionCount = 0;
    }

    // So ref does not get stuck in throwing animation when play ends
    public void clearThrowingAnimation()
    {
        clearTrajectory();
        animator.SetBool("isThrowing", false);
        animator.SetBool("isTimerRunning", false);
    }
}