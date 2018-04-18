using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Switcheroo : MonoBehaviour {

    GameObject myGo;
    SCR_PlayerTempStats myStats;
    SCR_Ranking myRanks;

	// Use this for initialization
	void Start () {

        myRanks = GameObject.FindGameObjectWithTag("RankingManager").GetComponent<SCR_Ranking>();
        for (int i = 0; i < myRanks.playerNum; i++)
        {
            if(myRanks.mySortingList[i].myPlace == myStats.myPlace-1)
            {
                //Switch!
                StartCoroutine(IESwitcheroo(myRanks.mySortingList[i].gameObject));
            }
            else if(myStats.myPlace ==1)
            {
                //Remove Item??
            }
        }

    }
	
    IEnumerator IESwitcheroo(GameObject _target)
    {
        Vector3 otherPos = _target.transform.position;
        yield return new WaitForSeconds(1.5f);
        _target.transform.position = myGo.transform.position;
        myGo.transform.position = otherPos;
    }

    public void SetInstancer(GameObject _go)
    {
        myGo = _go;
    }
}
