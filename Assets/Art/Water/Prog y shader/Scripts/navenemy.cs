using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class navenemy : MonoBehaviour {

    public bool tutorial;
    public int life;

    public GameObject target;

    public NavMeshAgent mynav;

    public GameObject enemyM;
    public bool chase = false;
	// Use this for initialization
	void Start () {
        StartCoroutine(SetActtive());
        enemyM.SetActive(false);
        //mynav = GetComponent<navenemy>();
        target = GameObject.FindGameObjectWithTag("Player");
        chase = false;

    }
	
	// Update is called once per frame
	void Update () {
        if (chase == true)
        {
            mynav.SetDestination(target.transform.position);
        }


	}

    IEnumerator MoveSet()
    {
        yield return new WaitForSeconds(5);

        chase = true;
    }

    IEnumerator SetActtive()
    {

        yield return new WaitForSeconds(1);
        //print("WaitAndPrint " + Time.time);
        StartCoroutine(MoveSet());
        enemyM.SetActive(true);

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "playerProjectile")
        {

            life--;
            print("auch");

        }

        if (collision.gameObject.tag == "Player")
        {

            chase = false;

            print("tocar?");
        }


        }
}
