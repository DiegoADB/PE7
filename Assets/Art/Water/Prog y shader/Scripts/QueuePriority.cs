using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueuePriority : MonoBehaviour {

    public Transform[] target;

    public float speed;

    public bool attack;
    bool stoyatacando = false;

    public Animator anim;

    public float readyAtck = 0;


    public GameObject projectile;
    public PriorityQue turn;
    //private List<GameObject> projectiles = new List<GameObject>();
    //public List<int> TurnNo = new List<int>();
    private float projectileVelo;

    void Start()
    {

        anim = GetComponent<Animator>();
        projectileVelo = 3;
        turn = GetComponentInParent<PriorityQue>();
    }

    void Update()
    {
        readyAtck+= 0.5f;

        /* if (Input.GetKeyDown(KeyCode.Y))
         {
             //attckE();
             /* float step = speed * Time.deltaTime;
              transform.position = Vector3.MoveTowards(transform.position, target[0].position, step);
              print("space key was pressed");
             attack = true;
         }
         if (Input.GetKeyDown(KeyCode.U))
         {
             //backE();
             float step = speed * Time.deltaTime;
             transform.position = Vector3.MoveTowards(transform.position, target[1].position, step);
             print("space key was pressed");
             attack = false;
         }
         if (Input.GetKeyDown(KeyCode.I))
         {
             //backE();
             float step = speed * Time.deltaTime;
             transform.position = Vector3.MoveTowards(transform.position, target[1].position, step);
             print("space key was pressed");
             anim.SetTrigger("Atkk1");
         }
         if (Input.GetKeyDown(KeyCode.O))
         {
             //backE();
             float step = speed * Time.deltaTime;
             transform.position = Vector3.MoveTowards(transform.position, target[1].position, step);
             print("space key was pressed");
             // attack = false;
             anim.SetTrigger("Attk2");
             GameObject bullet = (GameObject)Instantiate(projectile, transform.position, Quaternion.identity);

             bullet.transform.SetParent(transform, false);
            // acid.transform.Translate(new Vector2(0, 1) * Time.deltaTime*projectileVelo);

         }*/

        if (Input.GetKeyDown(KeyCode.O))
        {
            GameObject bullet=Instantiate(projectile, transform.position,transform.rotation);
            bullet.transform.SetParent(transform, true);
        }


            if (readyAtck>=500)
        {
            turn.Turno.Add(2);


            readyAtck = 0;
           
        }


        if (attack == true)
        {
            if (stoyatacando == false)
            {
                stoyatacando = true;
                StartCoroutine(Example());
            }
            attckE();
        }
        else
        {
            backE();

        }



    }

    void attckE()
    {
        float step = speed; // Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target[0].position, step);

        anim.SetTrigger("Atkk1");
    }

    void backE()
    {
       
        float step = speed; // Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target[1].position, step);

    }
    IEnumerator Example()
    {
        

        //turn.Turno.Remove(0);
        Debug.Log("alerta");
        turn.Turno.RemoveAt(0);
        yield return new WaitForSeconds(0.5f);
        
        attack = false;


        //turn.Turno.RemoveAt(0);
        //anim.SetTrigger("Atkk1");

        stoyatacando = false;
    }
    

}
