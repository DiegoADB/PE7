using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_CharacterStats : MonoBehaviour 
{

    [Header("Class Stats")]
    public float playerHP;
    public float strength;
    public float speed;
    public float handling;

    [Header("Stat Defaults")]
    public float startingHP = 100.0f;
    public float startingStr = 1.0f;
    public float startingSpd = 1.0f;
    public float startingHandling = 1.0f;

    private void Start()
    {
        playerHP = startingHP;
        strength = startingStr;
        speed = startingSpd;
        handling = startingHandling;
    }
}
