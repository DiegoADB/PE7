//Made by Diego Diaz
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;


public class SCR_CharacterMotor_Net : NetworkBehaviour
{
    public Behaviour[] componentsToDisable;
    public SCR_CharacterMotor helloMoto;
    public SCR_CharacterStats myStats;
    private void Start()
    {
     if(!isLocalPlayer)
        {
            for(int i = 0; i< componentsToDisable.Length ; i++)
            {
                componentsToDisable[i].enabled = false;
            }
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

    [ServerCallback]
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            myStats.playerHP -= 10 * collision.transform.GetComponent<SCR_CharacterMotor_Net>().myStats.strength;
            if(myStats.playerHP <= 0)
            {
                Rpc_DeathPlayer();
            }
            else
            {
                Rpc_DamagePlayer();

            }
        }
    }

    [ClientRpc]
    void Rpc_DeathPlayer()
    {
        //Debug.Log("morido");
        enabled = false;
        gameObject.SetActive(false);
    }

    [ClientRpc]
    void Rpc_DamagePlayer()
    {
        helloMoto.currentSpeed *= -0.5f;
    }
}