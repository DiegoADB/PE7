using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    public bool isInCinematic;
    public float Ospeed;
    public float speed=0;
    public float thrust=0;

    public bool isDodging;
    public float cdDodge;
    public Rigidbody rb;


    public GameObject rpgcanvas;
    public bool isrpgbatle=false;

    //public GameObject bestiary;
    public bool book;


    public GameObject projectile;

    //objetos para grab

    public Transform Pick1;
    public Transform Pick2;
    public Transform Pick3;
    public bool Pick1B;
    public bool Pick2B;
    public bool Pick3B;


    public Transform bulletSpawn;

    public float speedB = 20;

    public float lifeTime = 5;

    public GameObject[] hats;
    public int hatSelect;
    // Use this for initialization
    void Start ()
    {
        rb = GetComponent<Rigidbody>();
        isDodging = false;
        book = false;

        hats[0].SetActive(true);
        hats[1].SetActive(false);
        hats[2].SetActive(false);
        isInCinematic = false;

        //rpgcanvas = GameObject.FindGameObjectWithTag("rpgCanvas").GetComponent<GameObject>();

    }
	
	// Update is called once per frame
	void Update ()
    {

        Pick1.transform.RotateAround(transform.position, Vector3.up, Ospeed * Time.deltaTime);
        Pick2.transform.RotateAround(transform.position, Vector3.up, -Ospeed * Time.deltaTime);
        Pick3.transform.RotateAround(transform.position, Vector3.up, Ospeed * Time.deltaTime);

        if (isDodging == false && isrpgbatle==false && isInCinematic == false)
        {
            transform.Translate(speed * Input.GetAxis("Horizontal") * Time.deltaTime, 0f, speed * Input.GetAxis("Vertical") * Time.deltaTime);
        }


        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            hatSelect++;
        }
        
        if(hatSelect==0)
        {
            hats[0].SetActive(true);
            hats[1].SetActive(false);
            hats[2].SetActive(false);

        }
        if(hatSelect==1)
        {
            hats[0].SetActive(false);
            hats[1].SetActive(true);
            hats[2].SetActive(false);

        }
        if(hatSelect==2)
        {
            hats[0].SetActive(false);
            hats[1].SetActive(false);
            hats[2].SetActive(true);

        }
        if(hatSelect==3)
        {
            hatSelect = 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            book = !book;
        }
        /*if(book ==true)
        {
            bestiary.SetActive(true);
        }else
        {
            bestiary.SetActive(false);

        }
        */
        if (isrpgbatle==true)
        {
            rpgcanvas.SetActive(true);

        }else
        {
            rpgcanvas.SetActive(false);

        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            isrpgbatle = false;
        }

        /*if (Input.GetAxis("Horizontal") > 0 && Input.GetKeyDown(KeyCode.LeftShift) && isDodging==false)
        {
            //rb.AddForce(thrust,5, 0, ForceMode.Impulse);
            rb.velocity = new Vector3(thrust, 0, 0);
            isDodging = true;
            StartCoroutine(Example());

        }
        if (Input.GetAxis("Horizontal") < 0 && Input.GetKeyDown(KeyCode.LeftShift) && isDodging == false)
        {
            //rb.AddForce(thrust,5, 0, ForceMode.Impulse);
            isDodging = true;
            
            rb.velocity = new Vector3(-thrust, 0, 0);
            StartCoroutine(Example());

        }
        if (Input.GetAxis("Vertical") < 0 && Input.GetKeyDown(KeyCode.LeftShift) && isDodging == false)
        {
            //rb.AddForce(thrust,5, 0, ForceMode.Impulse);
            rb.velocity = new Vector3(0, 0, -thrust);
            isDodging = true;

            StartCoroutine(Example());


        }*/
        if (Input.GetAxis("Vertical") > 0 && Input.GetKeyDown(KeyCode.LeftShift) && isDodging == false)
        {
			//Vector3 empuje = transform.forward * thrust;	
			Vector3 empuje = transform.TransformDirection(new Vector3(0, 0, thrust));
            //rb.velocity = new Vector3(0, 0, thrust);
			rb.velocity = empuje;
            isDodging = true;

            StartCoroutine(Example());

        }
        /*if (Input.GetAxis("Vertical") > 0 && Input.GetAxis("Horizontal") > 0 && Input.GetKeyDown(KeyCode.LeftShift) )
        {
			rb.velocity = new Vector3(thrust,0,thrust);
        }
        if (Input.GetAxis("Vertical") < 0 && Input.GetAxis("Horizontal") > 0 && Input.GetKeyDown(KeyCode.LeftShift) )
        {
            rb.velocity = new Vector3(thrust, 0, -thrust);
        }
        if (Input.GetAxis("Vertical") < 0 && Input.GetAxis("Horizontal") < 0 && Input.GetKeyDown(KeyCode.LeftShift) )
        {
            rb.velocity = new Vector3(-thrust, 0, -thrust);
        }
        if (Input.GetAxis("Vertical") > 0 && Input.GetAxis("Horizontal") < 0 && Input.GetKeyDown(KeyCode.LeftShift) )
        {
            rb.velocity = new Vector3(-thrust, 0, thrust);
        }*/



        if (Input.GetKeyDown(KeyCode.Space))
        {
            Fire();

            //Instantiate(projectile,transform.position,transform.rotation);

            //instantiatedProjectile.velocity = transform.TransformDirection(new Vector3(0, 0, speedB));

        }

        //Debug.Log(Input.GetAxis("Horizontal"));
    }
    private void Fire()
    {

        GameObject bullet = Instantiate(projectile);

        Physics.IgnoreCollision(bullet.GetComponent<Collider>(), bulletSpawn.parent.GetComponent<Collider>());

        bullet.transform.position = bulletSpawn.position;

        Vector3 rotation = bullet.transform.rotation.eulerAngles;

        bullet.transform.rotation = Quaternion.Euler(rotation.x, transform.eulerAngles.y, rotation.z);

        bullet.GetComponent<Rigidbody>().AddForce(bulletSpawn.forward*speedB,ForceMode.Impulse);

        StartCoroutine(DestroyBulletAfterTime(bullet,lifeTime));
    }

    IEnumerator Example()
    {
        //print(Time.time);
        yield return new WaitForSeconds(cdDodge);
        //print(Time.time);
        isDodging = false;

    }
    private IEnumerator DestroyBulletAfterTime(GameObject bullet,float delay)
    {

        yield return new WaitForSeconds(delay);

        Destroy(bullet);

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "objcal1")
        {
            Debug.Log("Player touch");


            if (Input.GetKeyDown(KeyCode.E))
            {
                other.transform.position = Pick1.transform.position;
				other.transform.parent = Pick1;
                Pick1B = true;
            }
            

            //StartCoroutine(ChangeLevel());
            //ChangeLevel();
        }
        if (other.gameObject.tag == "objcal2")
        {
            Debug.Log("Player touch");


            if (Input.GetKeyDown(KeyCode.E))
            {
                other.transform.position = Pick2.transform.position;
                other.transform.parent = Pick2;
                Pick2B = true;


            }
            else if (Input.GetKeyDown(KeyCode.R))
            {

                other.transform.parent = null;
            }

            //StartCoroutine(ChangeLevel());
            //ChangeLevel();
        }
        if (other.gameObject.tag == "objcal3")
        {
            Debug.Log("Player touch");


            if (Input.GetKeyDown(KeyCode.E))
            {
                other.transform.position = Pick3.transform.position;
                other.transform.parent = Pick3;
                Pick3B = true;

            }
            else if (Input.GetKeyDown(KeyCode.R))
            {

                other.transform.parent = null;
            }

            //StartCoroutine(ChangeLevel());
            //ChangeLevel();
        }
        if (other.gameObject.tag == "callendary")
        {
            Destroy(Pick1.GetChild(0).Find("pieza1obj"));

        }



        }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "callendary")
        {
            if ((Pick1B == true))//&&(Pick2B==true)&&(Pick3B==true))
            {


                //Pick1.GetComponentInChildren<GameObject>();
                //Destroy();
                //transform.GetChild(0).GetChild(0).Find("pieza1");
                //GameObject.FindGameObjectWithTag("obj1Place").GetComponent<GameObject>().SetActive(true);
                
            }
            else
            {

                Debug.Log("no se puede poner las piezas aun");

            }


        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "enemy")
        {

            print("enemy touch");
            isrpgbatle = true;
        }
    }
}
