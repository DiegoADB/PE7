using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour {

    public GameObject spawnT;
    public GameObject enemyT;
    public GameObject camT;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator SetActtive()
    {

        yield return new WaitForSeconds(1);
        //print("WaitAndPrint " + Time.time);

        Instantiate(enemyT, spawnT.transform.position, spawnT.transform.rotation);
        StartCoroutine(CamT());
    }
    IEnumerator CamT()
    {
        yield return new WaitForSeconds(1);

        camT.SetActive(false);

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

            print("player entro al tutorial");
            StartCoroutine(SetActtive());

            camT.SetActive(true);

        }
        
        }
}
