using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Jugador : NetworkBehaviour {

	// Use this for initialization
	void Start () {
        this.enabled = base.isLocalPlayer;
    }
	
	// Update is called once per frame
	void Update () {
        //OBtenemos inputs
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        //Jugador
        transform.Translate(new Vector3(h, 0f, v) * 4.0f * Time.deltaTime);
    }
}
