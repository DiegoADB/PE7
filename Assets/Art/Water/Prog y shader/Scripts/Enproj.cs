using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enproj : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //transform.Translate(Vector3.up * Time.deltaTime, Space.World);

        transform.Translate(new Vector2(1, 0) * 50);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            print("player toach");

        }
        }
}
