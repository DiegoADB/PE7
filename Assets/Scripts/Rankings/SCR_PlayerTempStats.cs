using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_PlayerTempStats : MonoBehaviour {

    public int myPlace = 0;
    public int myScore = 0;
    int tempScore;
    public float distanceToNext;
    public GameObject[] myCheckpoints;
    //[HideInInspector]
    public GameObject nextTarget;
    [HideInInspector]
    public GameObject pastTarget;

    private void Start()
    {
        nextTarget = myCheckpoints[0];
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject==nextTarget)
        {
            myScore++;
            tempScore++;
            pastTarget = nextTarget;
            tempScore = myScore;

            if (tempScore>7)
            {
                tempScore = 0;
            }
            nextTarget = myCheckpoints[tempScore];
        }
    }



}
