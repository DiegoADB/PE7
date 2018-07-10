using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;
public class SCR_RedShell_Net : NetworkBehaviour {


    public GameObject myGo;
    SCR_PlayerTempStats myStats;
    SCR_Ranking myRanks;
    NavMeshAgent navAgent;
    // Use this for initialization
    public void Start()
    {
        SetInstancer();
        myStats = myGo.GetComponent<SCR_PlayerTempStats>();
        navAgent = gameObject.GetComponent<NavMeshAgent>();
        myRanks = GameObject.FindGameObjectWithTag("RankingManager").GetComponent<SCR_Ranking>();
        navAgent.Warp(new Vector3(myGo.transform.position.x, myGo.transform.position.y, myGo.transform.position.z) + gameObject.transform.forward);
        for (int i = 0; i < myRanks.playerNum; i++)
        {
            if (myRanks.mySortingList[i].myPlace == myStats.myPlace - 1)
            {
                //Switch!
                StartCoroutine(IERedShell(myRanks.mySortingList[i].gameObject));
            }
            else if (myStats.myPlace == 1)
            {
                //StartCoroutine(IERedShell(myRanks.mySortingList[7].gameObject));

                Destroy(gameObject);
            }
        }

    }

    IEnumerator IERedShell(GameObject _target)
    {
        yield return new WaitForEndOfFrame();
        navAgent.destination = _target.transform.position;

    }


    public void SetInstancer()
    {
        //List<GameObject> players = GameObject.FindGameObjectWithTag("RankingManager").GetComponent<SCR_Ranking>().players;
        //for (int i = 0; i < players.Count; i++)
        //{
        //    if (players[i].GetComponent<SCR_CharacterMotor_Net>().isLocalPlayer)
        //    {
        //        myGo = players[i];
        //        break;
        //    }
        //}
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(IEDestroy());
        }
    }

    IEnumerator IEDestroy()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}
