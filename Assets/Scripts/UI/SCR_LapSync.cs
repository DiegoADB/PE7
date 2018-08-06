using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCR_LapSync : MonoBehaviour
{

    public Text lapText;
    public Text myPlace;
    public int currentLap;
    public int lapTotal;
    public int checkpointsPassed;

	void Start ()
    {
        currentLap = 1;
        lapTotal = SCR_Ranking.numberOfLaps;
	}
	
	void Update ()
    {
        lapText.text = currentLap + "-" + lapTotal;
        myPlace.text = GetComponent<SCR_PlayerTempStats>().myPlace.ToString();

        checkpointsPassed = GetComponent<SCR_PlayerTempStats>().myScore;
        currentLap = Mathf.FloorToInt(checkpointsPassed / (SCR_PlayerTempStats.numberOfCheckpoints));
	}
}
