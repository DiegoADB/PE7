using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_CharacterStats : MonoBehaviour 
{
    private float playerHP;
    private float strength;
    private float speed;
    private float handling;

    public float GetCurrentHP()
    {
        return playerHP;
    }

    public void DamagePlayer(float _dmg)
    {
        playerHP -= _dmg;
        if (playerHP <= 0)
        {
            //TODO: Explode
        }
    }
}
