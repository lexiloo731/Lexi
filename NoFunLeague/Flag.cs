/*
 * Author: Alexia Nguyen
 * Description: Flag will stop moving after set time after entering field collider.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    Rigidbody2D rb;
    bool hasHit;
    float timeToTurnOffKinematic = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hasHit == false)
        {
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        StartCoroutine(TurnOffKinematic());
    }

    IEnumerator TurnOffKinematic()
    {
        yield return new WaitForSeconds(timeToTurnOffKinematic);
        hasHit = true;
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;

        // Call FlagLanded method in LevelManager when flag lands
        LevelManager levelManager = FindObjectOfType<LevelManager>();
        if (levelManager != null)
        {
            levelManager.FlagLanded();
        }
    }
}