using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCallendar : MonoBehaviour {

    public float Ospeed;

    public Transform target;



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        transform.RotateAround(target.transform.position, Vector3.up, Ospeed * Time.deltaTime);
    }


}
