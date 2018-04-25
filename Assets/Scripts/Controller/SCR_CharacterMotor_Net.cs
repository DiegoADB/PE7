//Made by Diego Diaz
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;


public class SCR_CharacterMotor_Net : NetworkBehaviour
{

    public SCR_CharacterMotor helloMoto;

    private void Start()
    {
     if(!isLocalPlayer)
        {
            this.enabled = false;
        }   

        helloMoto.MyStart();
        helloMoto.mainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    private void Update()
    {
        helloMoto.MyUpdate();
    }

    private void FixedUpdate()
    {
        helloMoto.MyFixedUpdate();
    }
}