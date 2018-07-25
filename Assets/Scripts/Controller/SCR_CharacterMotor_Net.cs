//Made by Diego Diaz
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;


public class SCR_CharacterMotor_Net : NetworkBehaviour
{
    public Behaviour[] componentsToDisable;
    public SCR_CharacterMotor helloMoto;
    public SCR_CharacterStats myStats;
    public bool orca;

    AudioSource SFX_player;
    public AudioClip[] SFX_clip;
    /*
     0 deslizar
     1 explosion 1
     2 explosion 2
     3 freno 1
     4 freno 2
     5 pasos 1
     6 pasos 2
     7 pasos 3
     8 idle
    */
    // public Transform RespawnPoint;
    [Header("Particles")]
    public GameObject myExplosion;
    public GameObject burnOutState;

    [Header("Camera")]
    public GameObject CameraPrefab;
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


       //SFX_player.GetComponent<AudioSource>();

        
        SCR_Ranking temp = GameObject.FindGameObjectWithTag("RankingManager").GetComponent<SCR_Ranking>();
        temp.players.Add(this.gameObject);
        temp.playerNum++;
        temp.AddPlayerList();
        helloMoto.MyStart();
        if (isLocalPlayer)
        {
            GameObject.FindGameObjectWithTag("MainCamera").SetActive(false);
            //GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioListener>().enabled = false;
            helloMoto.mainCamera = Instantiate(CameraPrefab, this.transform.position, Quaternion.identity).transform;

        }
       // helloMoto.mainCamera.position = this.transform.position;
    }
    private void Update()
    {
        if(isAlive && !orca)
            helloMoto.MyUpdate();
        if (Input.GetKeyDown(KeyCode.Alpha0))
            SCR_Disconnect.DisconnectFromMatch();
        
    }

    private void FixedUpdate()
    {
        //if(isAlive)
            helloMoto.MyFixedUpdate();
    }

    [Command]
    void Cmd_ChangePlayer()
    {
        Rpc_SpawnPlayer();
    }
    [ClientRpc]
    void Rpc_SpawnPlayer()
    {
        var conn = GetComponent<NetworkIdentity>().connectionToClient;
        var newPlayer = Instantiate<GameObject>(Resources.Load<GameObject>("Pingos/3"), transform.position, transform.rotation);
        Destroy(GetComponent<NetworkIdentity>().gameObject);
        NetworkServer.ReplacePlayerForConnection(conn, newPlayer, 0);
    }



    [ServerCallback]
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player") && isAlive && !orca)
        {
            myStats.playerHP -= 25 * collision.transform.GetComponent<SCR_CharacterMotor_Net>().myStats.strength;
            helloMoto.chocado = true;

            SCR_CharacterMotor temp = collision.transform.GetComponent<SCR_CharacterMotor>();
            if (helloMoto.currentSpeed >= temp.currentSpeed)
            {
                helloMoto.laFuerza = collision.impulse;
                Debug.Log(helloMoto.laFuerza + " fuerza");
                helloMoto.laDireccion = collision.contacts[0].normal;
                temp.myRB.AddForce(helloMoto.laFuerza * 100 * collision.transform.GetComponent<SCR_CharacterMotor_Net>().myStats.strength);
                helloMoto.currentSpeed = 0.0f;
                temp.currentSpeed = 0.0f;
            }

            Invoke("ReleaseChoke", 1.0f * collision.transform.GetComponent<SCR_CharacterMotor_Net>().myStats.handling);
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
        if (collision.transform.CompareTag("DeathCheck") && isAlive && !orca)
        {          
          Rpc_DeathPlayer();           
        }
    }

    void MayhemState()
    {
        helloMoto.mayhemState = true;
        //Invoke("Rpc_DeathPlayer", 5.0f);
    }

    [ClientRpc]
    void Rpc_DeathPlayer()
    {
        //Debug.Log("morido");

        //SFX_player.clip = SFX_clip[Random.Range(1, 2)];

        GameObject tempexplosion;
        tempexplosion = Instantiate(myExplosion);
        tempexplosion.transform.position = this.transform.position;
        //NetworkServer.Spawn(tempexplosion);
        isAlive = false;
        burnOutState.SetActive(true);
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
        burnOutState.SetActive(false);
        myStats.strength = myStats.startingStr;
        myStats.playerHP = myStats.startingHP;
        myStats.speed = myStats.startingSpd;
        myStats.handling = myStats.startingHandling;
        transform.GetChild(0).gameObject.SetActive(true);
        //Vector3 destination = GetComponent<SCR_PlayerTempStats>().nextTarget.transform.position - transform.position;
        //transform.LookAt((new Vector3(0, destination.y, 0)));
        helloMoto.GetMyRB().velocity = Vector3.zero;
        this.transform.position = helloMoto.savedPosition;
        if(helloMoto.mainCamera != null)
        {
            helloMoto.mainCamera.position = helloMoto.transform.position;
        }
    }


    void ReleaseChoke()
    {
        helloMoto.chocado = false;
    }
}