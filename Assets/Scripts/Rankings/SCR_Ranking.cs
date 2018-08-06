using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Ranking : MonoBehaviour {

    public List<GameObject> players;
    //SCR_PlayerTempStats[] myStats;
    List<SCR_PlayerTempStats> myStats;
    [HideInInspector]
    public List<SCR_PlayerTempStats> mySortingList;
    public int playerNum;

    public static string winnerNetID;
    public static int numberOfLaps = 2;
    public static bool b_raceFinished = false;


    void Start()
    {
        //myStats = new SCR_PlayerTempStats[playerNum];
        myStats = new List<SCR_PlayerTempStats>();
        mySortingList = new List<SCR_PlayerTempStats>();

        for (int i = 0; i < playerNum; i++)
        {
            Debug.Log(players.Count);
            myStats.Add(players[i].GetComponent<SCR_PlayerTempStats>());
            mySortingList.Add(myStats[i]);

        }
        StartCoroutine(CheckPositions());

    }

    public void AddPlayerList()
    {
        myStats.Add(players[playerNum-1].GetComponent<SCR_PlayerTempStats>());
        mySortingList.Add(myStats[playerNum-1]);
    }

    WaitForSeconds waitTime = new WaitForSeconds(0.25f);
    public IEnumerator CheckPositions()
    {
        for(int i =0; i<playerNum;i++)
        {
            myStats[i].distanceToNext = Vector3.Distance(players[i].transform.position, myStats[i].nextTarget.transform.position);
        }
        mySortingList.Sort(SortByScore);
        mySortingList.Sort(SortByDist);

        for (int i = 0; i < mySortingList.Count; i++)
        {
            
            mySortingList[i].myPlace = i+1;
        }
        yield return waitTime;

        StartCoroutine(CheckPositions());
    }

    public int SortByDist(SCR_PlayerTempStats a, SCR_PlayerTempStats b)
    {
        if(a.myScore==b.myScore)
        {
            return a.distanceToNext.CompareTo(b.distanceToNext);
        }
        else
        {
            return a.distanceToNext.CompareTo(a.distanceToNext);
        }
    }

    public int SortByScore(SCR_PlayerTempStats a, SCR_PlayerTempStats b)
    {

        return a.myScore.CompareTo(b.myScore);
    }

}
