/*
 * Author: Alexia Nguyen
 * Description: Player is able to sprint while stamina is available. Once stamina is depleted, cannot sprint until player catches breath.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStamina : MonoBehaviour
{
    public float totalStamina = 100, stamina;
    public float staminaDcrRate, staminaIncRate;
    public Slider staminaBar;
    bool isOutOfBreath;

    public Animator animator;

    // Start is called before the first frame update
    void Awake()
    {
        stamina = totalStamina;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOutOfBreath != true)
        {
            // Reduces stamina bar if player if running
            if (Input.GetKey(KeyCode.LeftShift) && stamina > 0 && !isOutOfBreath)
            {
                CharacterMovement.isRunning = true;
                stamina -= staminaDcrRate * Time.deltaTime;

                // If stamina reaches 0 then player is out of breath and cannot sprint 
                // until stamina refills
                if (stamina <= 0)
                {
                    isOutOfBreath = true;
                    CharacterMovement.isRunning = false;
                }
            }
            else
            {
                CharacterMovement.isRunning = false;
            }
        }

        // If stamina is refilled, allow the player to run again
        if (stamina >= totalStamina)
        {
            isOutOfBreath = false;
        }

        // Delay stamina recharge initially when out of breath
        if (isOutOfBreath && stamina == 0 && !Input.GetKey(KeyCode.LeftShift))
        {
            Invoke("CatchBreath", 1.5f);
            //Debug.Log("Caught Breath.");
        }
        // Recharges stamina after catching breath
        else if (isOutOfBreath && !Input.GetKey(KeyCode.LeftShift))
        {
            stamina += staminaIncRate * Time.deltaTime;
            //Debug.Log("Recharging stamina after catching breath.");
        }
        else if (isOutOfBreath && Input.GetKey(KeyCode.LeftShift))
        {
            // nothing happens
        }
        // Otherwise, passively regenerate stamina if not full
        else if (!isOutOfBreath && stamina != totalStamina && !Input.GetKey(KeyCode.LeftShift))
        {
            stamina += staminaIncRate * 0.40f * Time.deltaTime;
        }

        // Stamina stays in range of 0 - totalStamina
        stamina = Mathf.Clamp(stamina, 0, totalStamina);

        // Reduce stamina bar
        if (staminaBar != null)
        {
            staminaBar.value = stamina / totalStamina;
        }
    }

    void CatchBreath()
    {
        stamina += staminaIncRate * Time.deltaTime;
    }
}