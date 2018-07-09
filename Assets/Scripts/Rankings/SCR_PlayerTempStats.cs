using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class SCR_PlayerTempStats : NetworkBehaviour {

    public int myPlace = 0;
    public int myScore = 0;
    int tempScore;
    public float distanceToNext;
    [HideInInspector]
    public GameObject[] myCheckpoints;
    public GameObject nextTarget;
    [HideInInspector]
    public GameObject pastTarget;

    private void Start()
    {
        enabled = base.isLocalPlayer;
        myCheckpoints = GameObject.FindGameObjectWithTag("RankingTriggers").GetComponent<SCR_RankingTriggers>().triggers;
        nextTarget = myCheckpoints[0];
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject==nextTarget)
        {
            myScore++;
            tempScore++;
            pastTarget = nextTarget;
            Debug.Log(tempScore);
            if (tempScore>myCheckpoints.Length-1)
            {
                tempScore = 0;
            }
            nextTarget = myCheckpoints[tempScore];
        }
    }



}
