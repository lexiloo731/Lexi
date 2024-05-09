/*
 * Author: Alexia Nguyen
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyController : MonoBehaviour
{
    public int health = 3;
    public GameObject explosion;

    public float playerRange = 10f;

    public Rigidbody theRB;
    public float moveSpeed;

    public bool shouldShoot;
    public float fireRate = .5f;
    private float shotCounter;
    public GameObject bullet;
    public Transform firePoint;

    public int meleeDamage;

    bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) < playerRange)
            {
                Vector3 playerDirection = PlayerController.instance.transform.position - transform.position;

                if (shouldShoot)
                {
                    // Move enemy opposite direction of player
                    Vector3 oppositeDirection = -playerDirection.normalized;
                    theRB.velocity = oppositeDirection * moveSpeed;
                    theRB.AddForce(Vector3.down * 50);

                    shotCounter -= Time.deltaTime;
                    if (shotCounter <= 0)
                    {
                        Instantiate(bullet, firePoint.position, firePoint.rotation);
                        shotCounter = fireRate;
                    }
                }
                else
                {
                    theRB.velocity = playerDirection.normalized * moveSpeed;
                }
            }
            else
            {
                theRB.velocity = Vector3.zero;
                if (shouldShoot)
                {
                    theRB.AddForce(Vector3.down * 50);
                }
            }
        }
        else
        {
            theRB.velocity = Vector3.zero;
            theRB.useGravity = true;
            theRB.AddForce(Vector3.down * 50);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            player.TakeDamage(meleeDamage);

            if (!shouldShoot)
            {
                Destroy(gameObject);
                Instantiate(explosion, transform.position, transform.rotation);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            isDead = true;
        }
        else
        {
            AudioController.instance.PlayEnemyShot();
        }
    }
}
