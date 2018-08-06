using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class SCR_PlayerTempStats : NetworkBehaviour {

    public int myPlace = 0;
    public int myScore = 0;
    public int tempScore;
    public float distanceToNext;
    [HideInInspector]
    public GameObject[] myCheckpoints;
    public GameObject nextTarget;
    [HideInInspector]
    public GameObject pastTarget;

    public Text resultText;
    public static int numberOfCheckpoints;

    private void Start()
    {
        enabled = base.isLocalPlayer;
        myCheckpoints = GameObject.FindGameObjectWithTag("RankingTriggers").GetComponent<SCR_RankingTriggers>().triggers;
        numberOfCheckpoints = myCheckpoints.Length;
        nextTarget = myCheckpoints[0];
    }

    private void Update()
    {
        if (!SCR_Ranking.b_raceFinished)
            return;
        ChangeEndText();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == nextTarget)
        {
            myScore++;
            tempScore++;
            pastTarget = nextTarget;
            if (tempScore > myCheckpoints.Length - 1)
            {
                tempScore = 0;
            }
            nextTarget = myCheckpoints[tempScore];

            if (myScore == (numberOfCheckpoints * SCR_Ranking.numberOfLaps) && !SCR_Ranking.b_raceFinished)
            {
                SCR_Ranking.winnerNetID = netId.ToString();
                SCR_Ranking.b_raceFinished = true;
            }
        }
    }


    public void ChangeEndText()
    {
        if(netId.ToString() == SCR_Ranking.winnerNetID)
        {
            resultText.text = "A winner is you";
        }
        else
        {
            resultText.text = "Loser loser chicken... loser";
        }
        StartCoroutine(KickPlayers());
    }

    public void TestChangeText()
    {
        resultText.text = "OGH";
    }

    IEnumerator KickPlayers()
    {
        yield return new WaitForSeconds(3.0f);
        SCR_Disconnect.DisconnectFromMatch();
        SCR_Ranking.b_raceFinished = false;
    }

}
