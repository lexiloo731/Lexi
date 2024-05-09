/*
 * Author: Alexia Nguyen
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private float timeBtwAttack;
    public float startTimeBtwAttack;

    public Transform attackPos;
    public LayerMask whatisEnemy;
    public float attackRange;
    public int damage;
    public KeyCode punch;
    public KeyCode kick;
    
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        if(timeBtwAttack <= 0)
        {
            if(Input.GetKeyDown(punch))
            {
                Debug.Log("punch");
                anim.SetTrigger("Punch");
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatisEnemy);
                for(int i = 0; i < enemiesToDamage.Length; i++)
                {
                    enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(damage);
                }
                timeBtwAttack = startTimeBtwAttack;
            }

            if(Input.GetKeyDown(kick))
            {
                Debug.Log("kick");
                anim.SetTrigger("Kick");
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatisEnemy);
                for(int i = 0; i < enemiesToDamage.Length; i++)
                {
                    enemiesToDamage[i].GetComponent<Enemy>().TakeDamage((damage * 2));
                }
                timeBtwAttack = startTimeBtwAttack * 2;
            }
        }
        else
        {
            timeBtwAttack -= Time.deltaTime;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}