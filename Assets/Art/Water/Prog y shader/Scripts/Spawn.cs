using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour {

    public GameObject PlayPref;


	// Use this for initialization
	void Start () {
        Instantiate(PlayPref,transform.position,transform.rotation);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
