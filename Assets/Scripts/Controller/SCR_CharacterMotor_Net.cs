//Made by Diego Diaz
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;


public class SCR_CharacterMotor_Net : NetworkBehaviour
{
    public Behaviour[] componentsToDisable;
    public SCR_CharacterMotor helloMoto;
    public SCR_CharacterStats myStats;
    public bool usingItem;

    AudioSource SFX_player;
    public AudioClip[] SFX_clip;

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

        helloMoto.savedPosition = transform.position;
        SCR_Ranking temp = GameObject.FindGameObjectWithTag("RankingManager").GetComponent<SCR_Ranking>();
        temp.players.Add(this.gameObject);
        temp.playerNum++;
        temp.AddPlayerList();
        helloMoto.MyStart();
        if (isLocalPlayer)
        {
            GameObject.FindGameObjectWithTag("MainCamera").SetActive(false);
            helloMoto.mainCamera = Instantiate(CameraPrefab, this.transform.position, Quaternion.identity).transform;

        }
    }
    private void Update()
    {
        if (isAlive && !usingItem)
        {
            helloMoto.MyUpdate();
            if (Input.GetButtonDown("P1_LB") && !usingItem)
                Rpc_DeathPlayer();
            if (Input.GetKeyDown(KeyCode.Escape))
                SCR_Disconnect.DisconnectFromMatch();
        }
    }


    [Command]
    void Cmd_ChangePlayerType(string _pingoName)
    {
        var conn = GetComponent<NetworkIdentity>().connectionToClient;
        var newPlayer = Instantiate<GameObject>(Resources.Load<GameObject>("Pingos/" + _pingoName), transform.position, transform.rotation);
        Destroy(GetComponent<NetworkIdentity>().gameObject);
        NetworkServer.ReplacePlayerForConnection(conn, newPlayer, 0);
    }

    private void FixedUpdate()
    {
        helloMoto.MyFixedUpdate();
    }

    private void OnApplicationQuit()
    {
        SCR_Disconnect.DisconnectFromMatch();
    }

    [ServerCallback]
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player") && isAlive && !usingItem)
        {
            myStats.playerHP -= 25 * collision.transform.GetComponent<SCR_CharacterMotor_Net>().myStats.strength;
            helloMoto.chocado = true;

            SCR_CharacterMotor temp = collision.transform.GetComponent<SCR_CharacterMotor>();
            if (helloMoto.currentSpeed >= temp.currentSpeed)
            {
                helloMoto.laFuerza = collision.impulse;
                helloMoto.laDireccion = collision.contacts[0].normal;
                temp.myRB.AddForce(helloMoto.laFuerza * 100 * collision.transform.GetComponent<SCR_CharacterMotor_Net>().myStats.strength);
                helloMoto.currentSpeed = 0.0f;
                temp.currentSpeed = 0.0f;
            }

            Invoke("ReleaseChoke", 1.0f * collision.transform.GetComponent<SCR_CharacterMotor_Net>().myStats.handling);
            if (myStats.playerHP <= 0)
            {
                Rpc_DeathPlayer();
            }
        }
    }

    [ServerCallback]
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.CompareTag("DeathCheck") && isAlive && !usingItem)
        {
            Rpc_DeathPlayer();
        }
        else if (collision.transform.CompareTag("Red") && isAlive && !usingItem && collision.transform.GetComponent<SCR_RedShell_Net>().instancer != gameObject)
        {
            Rpc_DeathPlayer();
            NetworkServer.Destroy(collision.gameObject);
        }
    }

    [ClientRpc]
    public void Rpc_Boost()
    {
        helloMoto.SpeedBoost(1);
    }

    [ClientRpc]
    void Rpc_DeathPlayer()
    {
        GameObject tempexplosion;
        tempexplosion = Instantiate(myExplosion);
        tempexplosion.AddComponent<SCR_Destroy>();
        tempexplosion.transform.position = this.transform.position;
        isAlive = false;
        burnOutState.SetActive(true);
        Invoke("Rpc_Respawn", 3.0f);
        transform.GetChild(0).gameObject.SetActive(false);
    }

    [ClientRpc]
    void Rpc_DamagePlayer()
    {
        helloMoto.currentSpeed *= -0.5f;
    }

    [ClientRpc]
    void Rpc_Respawn()
    {
        helloMoto.RandomAddOn();
        if(GetComponent<SCR_PlayerTempStats>().pastTarget != null)
            helloMoto.savedPosition = GetComponent<SCR_PlayerTempStats>().pastTarget.transform.position;
        transform.position = helloMoto.savedPosition;
        Invoke("Rpc_RespawnPosition", 0.1f);
      
    }
    [ClientRpc]
    void Rpc_RespawnPosition()
    {
        helloMoto.myRB.velocity = Vector3.zero;
        helloMoto.currentSpeed = 0;
        isAlive = true;
        burnOutState.SetActive(false);
        myStats.strength = myStats.startingStr;
        myStats.playerHP = myStats.startingHP;
        myStats.speed = myStats.startingSpd;
        myStats.handling = myStats.startingHandling;
        transform.GetChild(0).gameObject.SetActive(true);
        helloMoto.myRB.velocity = Vector3.zero;
        if (helloMoto.mainCamera != null)
        {
            helloMoto.mainCamera.position = helloMoto.transform.position;
        }
    }



    void ReleaseChoke()
    {
        helloMoto.chocado = false;
    }
}