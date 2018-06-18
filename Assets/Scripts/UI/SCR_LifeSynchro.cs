using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_LifeSynchro : MonoBehaviour {

    public GameObject[] lifePrefabs;
    [Range(0, 100)]
    public int playerTotalHP;

    [Range(0, 100)]
    public int currentHP;

    public int loopInt = 0;
    
	void Start () {

        playerTotalHP = 100;
		
	}

	void Update ()
    {
        for(int i = 0; i < lifePrefabs.Length; i++)
        {
            lifePrefabs[i].SetActive(false);
        }

        if(playerTotalHP == 100)
        {
            for (int i = 0; i < lifePrefabs.Length; i++)
            {
                lifePrefabs[i].SetActive(true);
            }
        }

        loopInt = Mathf.RoundToInt((playerTotalHP / 100.0f) * 6);

        for (int i = 0; i < loopInt; i++)
        {
            lifePrefabs[i].SetActive(true);
        }
		
        if(playerTotalHP == 0)
        {
            for(int i = 0; i < lifePrefabs.Length; i++)
            {
                lifePrefabs[i].SetActive(false);
            }
        }

	}
}
