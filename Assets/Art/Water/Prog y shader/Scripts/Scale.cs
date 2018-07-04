using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scale : MonoBehaviour {

    public bool startGrow = true;

	// Use this for initialization
	void Start () {
        StartCoroutine(SmallSpawn());
        Destroy(gameObject,2);
    }
	
	// Update is called once per frame
	void Update () {
        if(startGrow==true)
        {

            transform.localScale += new Vector3(5, 5, 5)*Time.deltaTime;

        }else
        {
            transform.localScale -= new Vector3(5, 5, 5) * Time.deltaTime;


        }

    }

    IEnumerator SmallSpawn()
    {
        
            yield return new WaitForSeconds(1);
        //print("WaitAndPrint " + Time.time);

        startGrow = false;
        
    }
}
