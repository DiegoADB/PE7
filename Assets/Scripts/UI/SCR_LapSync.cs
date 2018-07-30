﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCR_LapSync : MonoBehaviour
{

    public Text lapText;
    public int currentLap;
    public int lapTotal;
    public int checkpointsPassed;

	void Start ()
    {
        currentLap = 1;
        lapTotal = SCR_Ranking.numberOfLaps;
        checkpointsPassed = GetComponent<SCR_PlayerTempStats>().tempScore;
	}
	
	void Update ()
    {
        lapText.text = currentLap + "-" + lapTotal;

        currentLap = Mathf.FloorToInt(checkpointsPassed / SCR_PlayerTempStats.numberOfCheckpoints) + 1;
	}
}
