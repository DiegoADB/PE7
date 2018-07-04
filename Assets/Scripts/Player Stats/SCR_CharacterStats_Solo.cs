using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCR_CharacterStats_Solo : MonoBehaviour
{
    [SerializeField]
    private Image playerHPBar;

    [Header("Class Stats")]
    public float strength;
    public float speed;
    public float handling;
    public float playerHP;


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

    private void Update()
    {
        //playerHPBar.fillAmount = playerHP / 100.0f;
    }
}
