using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePieces : MonoBehaviour {

    public GameObject wall;

    private Vector3 starPos;

    private Vector3 endPos;

    private float distance = 30f;

    private float lerpTime = 5;

    private float currentLerpTime = 0;

    public bool keyHit = false;


    // Use this for initialization
    void Start () {

       
        endPos = wall.transform.position + Vector3.down * distance;

	}
	
	// Update is called once per frame
	void Update () {

        starPos = wall.transform.position;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {

            keyHit = true;

        }
        if(keyHit == true)
        {

            currentLerpTime += Time.deltaTime;
            if(currentLerpTime>=lerpTime)
            {
                currentLerpTime = lerpTime;
            }
            float Perc = currentLerpTime / lerpTime;
            wall.transform.position = Vector3.Lerp(starPos, endPos, Perc);

        }
	}
}
