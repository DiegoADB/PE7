using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class SCR_RedShell : MonoBehaviour {

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
        for (int i = 0; i < myRanks.playerNum; i++)
        {
            if (myRanks.mySortingList[i].myPlace == myStats.myPlace - 1)
            {
                //Switch!
                StartCoroutine(IERedShell(myRanks.mySortingList[i].gameObject));
            }
            else if (myStats.myPlace == 1)
            {
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
}
