using UnityEngine;
using System.Collections;

public class SCR_CharacterMotor_Solo : MonoBehaviour
{
    //public Behaviour[] componentsToDisable;
    public SCR_CharacterMotor helloMoto;
    public SCR_CharacterStats myStats;
    //public Transform RespawnPoint;
    public bool orca;
    public GameObject myExplosion;
    public GameObject burnOutState;

    public GameObject CameraPrefab;
    bool isAlive = true;

    
    private void Start()
    {
        SCR_Ranking temp = GameObject.FindGameObjectWithTag("RankingManager").GetComponent<SCR_Ranking>();
        temp.players.Add(this.gameObject);
        temp.playerNum++;
        temp.AddPlayerList();
        helloMoto.MyStart();

        GetComponentInChildren<Canvas>().enabled = true;
        
        helloMoto.mainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        //Debug.Log(helloMoto.mainCamera.name);
    }
    private void Update()
    {
        if (isAlive)
        {
            helloMoto.SaveLastPosition();
            helloMoto.MyUpdate();
        }
        //ChooseCharacter();
    }

    //void ChooseCharacter()
    //{
    //    if (Input.GetKeyDown(KeyCode.Alpha1))
    //    {
    //        Cmd_ChangePlayerType("Normal");
    //    }
    //    if (Input.GetKeyDown(KeyCode.Alpha2))
    //    {
    //        Cmd_ChangePlayerType("Heavy");
    //    }
    //    if (Input.GetKeyDown(KeyCode.Alpha3))
    //    {
    //        Cmd_ChangePlayerType("Lucky");
    //    }
    //}

    //void Cmd_ChangePlayerType(string _pingoName)
    //{
    //    var newPlayer = Instantiate<GameObject>(Resources.Load<GameObject>("Pingos/" + _pingoName), transform.position, transform.rotation);
    //}

    private void FixedUpdate()
    {
        //if(isAlive)
            helloMoto.MyFixedUpdate();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player") && isAlive)
        {
            myStats.playerHP -= 10 * collision.transform.GetComponent<SCR_CharacterStats>().strength;
            helloMoto.chocado = true;

            SCR_CharacterMotor temp = collision.transform.GetComponent<SCR_CharacterMotor>();
            if (helloMoto.currentSpeed >= temp.currentSpeed)
            {
                helloMoto.laFuerza = collision.impulse;
                Debug.Log(helloMoto.laFuerza + " fuerza");
                helloMoto.laDireccion = collision.contacts[0].normal;
                temp.myRB.AddForce(helloMoto.laFuerza * 100 * collision.transform.GetComponent<SCR_CharacterStats>().strength);
                helloMoto.currentSpeed = 0.0f;
                temp.currentSpeed = 0.0f;
            }

            Invoke("ReleaseChoke", 1.0f * collision.transform.GetComponent<SCR_CharacterStats>().handling);
            if (myStats.playerHP <= 0)
            {
                Rpc_DeathPlayer();
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.CompareTag("DeathCheck") && isAlive)
        {            
          Rpc_DeathPlayer();           
        }
    }


    void Rpc_DeathPlayer()
    {
        //Debug.Log("morido");
        GameObject tempexplosion;
        tempexplosion = Instantiate(myExplosion);
        tempexplosion.AddComponent<SCR_Destroy>();
        tempexplosion.transform.position = this.transform.position;
        isAlive = false;
        burnOutState.SetActive(true);
        Invoke("Rpc_Respawn", 3.0f);
        transform.GetChild(0).gameObject.SetActive(false);
    }

    void Rpc_DamagePlayer()
    {
        helloMoto.currentSpeed *= -0.5f;
    }

    void Rpc_Respawn()
    {
        helloMoto.RandomAddOn();
        
        Invoke("Rpc_RespawnPosition", 0.1f);
    }

    void Rpc_RespawnPosition()
    {
        helloMoto.currentSpeed = 0;
        isAlive = true;
        burnOutState.SetActive(false);
        myStats.strength = myStats.startingStr;
        myStats.playerHP = myStats.startingHP;
        myStats.speed = myStats.startingSpd;
        myStats.handling = myStats.startingHandling;
        transform.GetChild(0).gameObject.SetActive(true);
        helloMoto.GetMyRB().velocity = Vector3.zero;
        Debug.Log("###### muerte " + helloMoto.savedPosition);
        transform.position = helloMoto.savedPosition;
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