using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SCR_RedShell_Net : MonoBehaviour {


    [SerializeField]
    GameObject myGo;
    SCR_PlayerTempStats myStats;
    SCR_Ranking myRanks;
    NavMeshAgent navAgent;
    // Use this for initialization
    void Start()
    {
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


    public void SetInstancer(GameObject _go)
    {
        myGo = _go;
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
