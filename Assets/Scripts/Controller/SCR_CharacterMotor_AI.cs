//Made by Diego Diaz
using UnityEngine;
using System.Collections;


public class SCR_CharacterMotor_AI : MonoBehaviour
{
    public SCR_CharacterMotor helloMoto;
    public SCR_CharacterStats myStats;
    public bool orca;
    public GameObject[] rankings;
    GameObject nextTargets;
    int count = 0;
    float timer = 0.0f;
    float timer2 = 0.0f;
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
    bool isAlive = true;
    
    
    private void Start()
    {
        helloMoto.playerPrefix = "P2_";
        helloMoto.MyStart();
        helloMoto.isIA = true;
        nextTargets = rankings[count];
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
        //dot1 = Vector3.Dot(this.transform.position, rankings[count].transform.position);
        //dot2 = Vector3.Dot(this.transform.right, rankings[count].transform.position);
        this.transform.LookAt(nextTargets.transform.position);
        // Debug.Log("Pinguino " + this.transform.name + " va a " +rankings[count]);
        timer += Time.deltaTime;
        timer2 += Time.deltaTime;

        if (timer >= 1.5f)
        {
            Debug.Log("entro1");
            if (Random.Range(1, 100) <= 70)
            {
                if (Random.Range(1, 100) <= 70)
                {
                    Debug.Log("Pinguino " + this.transform.name + "Entro al hori false");
                    helloMoto.horizontalInput = -0.5f;
                }
                else
                {
                    Debug.Log("Pinguino " + this.transform.name + "Entro al hori true");

                    helloMoto.horizontalInput = 0.5f;

                }
            }
            else
            {
                helloMoto.horizontalInput = 0.0f;
            }

            timer = 0.0f;
        }
        else
        {
            helloMoto.horizontalInput = 0.0f;   
        }

        helloMoto.verticalInput = 1.0f; 

        if(timer2 >= 2.0f)
        {
            Debug.Log("entro2");
            if (Random.Range(1, 100) <= 70)
            {
                Debug.Log("Pinguino " + this.transform.name + "Entro al a false");
                helloMoto.aButton = false;
            }
            else
            {
                helloMoto.aButton = true;
            }
            timer2 = 0.0f;
        }
        else
        {
            helloMoto.aButton = true;
        }
         
        
            helloMoto.MyFixedUpdate();
    }

  
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player") && isAlive && !orca)
        {
            myStats.playerHP -= 10 * collision.transform.GetComponent<SCR_CharacterMotor_AI>().myStats.strength;
            helloMoto.chocado = true;

            SCR_CharacterMotor temp = collision.transform.GetComponent<SCR_CharacterMotor>();
            if (helloMoto.currentSpeed >= temp.currentSpeed)
            {
                helloMoto.laFuerza = collision.impulse;
                //Debug.Log(helloMoto.laFuerza + " fuerza");
                helloMoto.laDireccion = collision.contacts[0].normal;
                temp.myRB.AddForce(helloMoto.laFuerza * 100 * collision.transform.GetComponent<SCR_CharacterMotor_AI>().myStats.strength);
                helloMoto.currentSpeed = 0.0f;
                temp.currentSpeed = 0.0f;
            }

            Invoke("ReleaseChoke", 1.0f * collision.transform.GetComponent<SCR_CharacterMotor_AI>().myStats.handling);
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

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.CompareTag("DeathCheck") && isAlive && !orca)
        {
            Debug.Log("### MUERTE: " + collision.transform.name);
            Rpc_DeathPlayer();
        }

        if (collision.transform.name == rankings[count].transform.name && isAlive)
        {
           
           count++;
           nextTargets = rankings[count];
           
            if (count >= rankings.Length)
            {
                count = 0;
            }
            //Debug.Log("Pinguino " + this.transform.name + " va a " + count);
        }
    }

    void MayhemState()
    {
        helloMoto.mayhemState = true;
        //Invoke("Rpc_DeathPlayer", 5.0f);
    }

   
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

    
    void Rpc_DamagePlayer()
    {
        helloMoto.currentSpeed *= -0.5f;
    }

   
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
        this.transform.position = new Vector3(rankings[count].transform.position.x, rankings[count].transform.position.y +10, rankings[count].transform.position.z);
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