using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2D : MonoBehaviour {
    public int jumpSpeed;
    public PriorityQue turn;

    public float attckRdy =0;

    // Use this for initialization
    void Start () {
        turn = GetComponentInParent<PriorityQue>();

	}
	
	// Update is called once per frame
	void Update () {

        //transform.Translate(Vector3.up * jumpSpeed * Time.smoothDeltaTime);
        //print(transform.position.x);

        attckRdy += 0.5f;


        if(attckRdy>=300)
        {

            turn.Turno.Add(1);
            attckRdy = 0;
        }

    }

    public void PlayerAttk()
    {

        turn.Turno.RemoveAt(0);
    }
}
