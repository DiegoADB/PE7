using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_CharacterStats : MonoBehaviour 
{
    private SCR_CharacterMotor motor;

    private float playerHP;
    private float strength;
    private float speed;
    private float handling;

    [Header("Stats")]
    public float startingHP = 100.0f;
    public float baseDamage = 5.0f;

    private void Start()
    {
        motor = GetComponent<SCR_CharacterMotor>();
        playerHP = startingHP;
    }

    public float GetCurrentHP()
    {
        return playerHP;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Player"))
        {
            DamagePlayer(baseDamage * motor.currentSpeed);
            motor.GetMyRB().AddForceAtPosition(transform.up * 40 * motor.currentSpeed, transform.position);
        }
    }

    public void DamagePlayer(float _dmg)
    {
        playerHP -= _dmg;
        Debug.Log(transform.name + playerHP);
        if (playerHP <= 0)
        {
            //TODO: Explode
        }
    }
}
