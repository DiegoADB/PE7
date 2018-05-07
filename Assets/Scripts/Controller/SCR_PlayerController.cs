using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_PlayerController : MonoBehaviour {

    SCR_CharacterMotor heck;

    private void Start()
    {
        heck = GetComponent<SCR_CharacterMotor>();
        heck.MyStart();

    }
    private void Update()
    {
        heck.MyUpdate();
    }
    private void FixedUpdate()
    {
        heck.MyFixedUpdate();

    }
}
