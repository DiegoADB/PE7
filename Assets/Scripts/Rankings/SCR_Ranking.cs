using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Ranking : MonoBehaviour {

    public GameObject[] players;
    SCR_PlayerTempStats[] myStats;
    List<SCR_PlayerTempStats> mySortingList;
    public int playerNum;


    void Start()
    {
        myStats = new SCR_PlayerTempStats[playerNum];
        mySortingList = new List<SCR_PlayerTempStats>();

        for (int i = 0; i < playerNum; i++)
        {
            myStats[i] = players[i].GetComponent<SCR_PlayerTempStats>();
            mySortingList.Add(myStats[i]);

        }
        StartCoroutine(CheckPositions());

    }

    IEnumerator CheckPositions()
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

        yield return new WaitForSeconds(0.25f);

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
