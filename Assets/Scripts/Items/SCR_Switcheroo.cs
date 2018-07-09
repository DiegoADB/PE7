using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class SCR_Switcheroo : NetworkBehaviour {

    public GameObject myGo;
    SCR_PlayerTempStats myStats;
    SCR_Ranking myRanks;
    public float timeBeforeChange = 1.5f;
	// Use this for initialization
	public void Start () {
        myStats = myGo.GetComponent<SCR_PlayerTempStats>();
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
                Destroy(gameObject);
            }
        }

    }
	
    IEnumerator IESwitcheroo(GameObject _target)
    {
        Vector3 otherPos = _target.transform.position;
        Quaternion otherRot = _target.transform.rotation;
        yield return new WaitForSeconds(timeBeforeChange);
        _target.transform.position = myGo.transform.position;
        //Could be removed
        _target.transform.rotation = myGo.transform.rotation;
        myGo.transform.rotation = otherRot;
        myGo.transform.position = otherPos;
        Destroy(gameObject);
    }
    public void SetInstancer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                myGo = players[i];
                break;
            }
        }
    }
}
