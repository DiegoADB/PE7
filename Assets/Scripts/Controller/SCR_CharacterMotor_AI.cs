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
    RaycastHit myRay;
    int randomx;
    int randomz;


    private void Start()
    {
        helloMoto.playerPrefix = "P2_";
        helloMoto.MyStart();
        helloMoto.isIA = true;
        nextTargets = rankings[count];
        myRay = new RaycastHit();
        randomx = Random.Range(-2, 2);
        randomz = Random.Range(-2, 2);
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
        this.transform.LookAt(new Vector3(nextTargets.transform.position.x + randomx, 
                                            nextTargets.transform.position.y,
                                            nextTargets.transform.position.z + randomz));
        // Debug.Log("Pinguino " + this.transform.name + " va a " +rankings[count]);
        timer += Time.deltaTime;
        timer2 += Time.deltaTime;
        
        Debug.DrawRay(this.transform.position, this.transform.forward * 10, Color.cyan);
        if (Physics.Raycast(this.transform.position, this.transform.forward,out myRay, 10.0f))
        {
            //Debug.Log(myRay.transform.name);
            if(myRay.transform.CompareTag("Obstacle"))
            {
                helloMoto.horizontalInput = 1.0f;
                //helloMoto.verticalInput = -1.0f;
                helloMoto.drifting = true;
                Debug.Log("Pinguino " + this.transform.name + " +1 pego con " + myRay.transform.name);                             
            }
        }
        else
        {
            helloMoto.drifting = false;
            helloMoto.horizontalInput = 0.0f;
        }

        helloMoto.verticalInput = 1.0f; 

        
        helloMoto.aButton = true;
      
        
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
            Rpc_DeathPlayer();
        }

        if (collision.transform.name == rankings[count].transform.name && isAlive)
        {
           
           count++;
           nextTargets = rankings[count];
           
            if (count >= rankings.Length-1)
            {
                count = 0;
            }
            randomx = Random.Range(-2, 2);
            randomz = Random.Range(-2, 2);
            if (count == 4 || count == 3)
            {
                randomx = 0;
                randomz = 0;
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