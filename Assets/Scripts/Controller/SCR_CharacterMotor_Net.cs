//Made by Diego Diaz
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;


public class SCR_CharacterMotor_Net : NetworkBehaviour
{
    public Behaviour[] componentsToDisable;
    public SCR_CharacterMotor helloMoto;
    public SCR_CharacterStats myStats;
   // public Transform RespawnPoint;
    public GameObject myExplosion;
    bool isAlive = true;
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
        helloMoto.mainCamera.position = transform.position;
    }

    private void Update()
    {
        if(isAlive)
            helloMoto.MyUpdate();
    }

    private void FixedUpdate()
    {
        if(isAlive)
            helloMoto.MyFixedUpdate();
    }

    [ServerCallback]
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player") && isAlive)
        {
            myStats.playerHP -= 10 * collision.transform.GetComponent<SCR_CharacterMotor_Net>().myStats.strength;
            if (helloMoto.mayhemState)
            {
                Rpc_DeathPlayer();
            }
            else if (myStats.playerHP <= 0)
            {
                MayhemState();
            }


            
        }
    }

    [ServerCallback]
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.CompareTag("DeathCheck") && isAlive)
        {          
          Rpc_DeathPlayer();           
        }
    }

    void MayhemState()
    {
        helloMoto.mayhemState = true;
        Invoke("Rpc_DeathPlayer", 5.0f);
    }

    [ClientRpc]
    void Rpc_DeathPlayer()
    {
        //Debug.Log("morido");
        GameObject tempexplosion;
        tempexplosion = Instantiate(myExplosion);
        tempexplosion.transform.position = this.transform.position;
        NetworkServer.Spawn(tempexplosion);
        isAlive = false;
        Invoke("Rpc_Respawn", 3.0f);
        transform.GetChild(0).gameObject.SetActive(false);
      //  gameObject.SetActive(false);
    }

    [ClientRpc]
    void Rpc_DamagePlayer()
    {
        helloMoto.currentSpeed *= -0.5f;
    }

    [ClientRpc]
    void Rpc_Respawn()
    {
        helloMoto.mayhemState = false;
        helloMoto.currentSpeed = 0;
        isAlive = true;
        myStats.strength = myStats.startingStr;
        myStats.playerHP = myStats.startingHP;
        myStats.speed = myStats.startingSpd;
        myStats.handling = myStats.startingHandling;
        transform.GetChild(0).gameObject.SetActive(true);
        //Vector3 destination = GetComponent<SCR_PlayerTempStats>().nextTarget.transform.position - transform.position;
        //transform.LookAt((new Vector3(0, destination.y, 0)));
        helloMoto.GetMyRB().velocity = Vector3.zero;
        this.transform.position = helloMoto.savedPosition;
        helloMoto.mainCamera.position = helloMoto.transform.position;

    }
}