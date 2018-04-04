//Made by Diego Diaz
//diegodiaz@golstats.com
using UnityEngine;

public class SCR_SpeedBoost : MonoBehaviour 
{
    public float boostDuration;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<SCR_CharacterMotor>())
            other.GetComponent<SCR_CharacterMotor>().SpeedBoost(boostDuration);
    }

}
