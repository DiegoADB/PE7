using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;
public class SCR_RedShell_Net : NetworkBehaviour {

    public GameObject instancer;
    SCR_PlayerTempStats myStats;
    SCR_Ranking myRanks;
    NavMeshAgent navAgent;
    float timer = 20f;
    // Use this for initialization
    public void Start2()
    {
        //SetInstancer();
        myStats = instancer.GetComponent<SCR_PlayerTempStats>();
        navAgent = gameObject.GetComponent<NavMeshAgent>();
        myRanks = GameObject.FindGameObjectWithTag("RankingManager").GetComponent<SCR_Ranking>();
        navAgent.Warp(new Vector3(instancer.transform.position.x, instancer.transform.position.y, instancer.transform.position.z) + gameObject.transform.forward);
        for (int i = 0; i < myRanks.playerNum; i++)
        {
            if (myRanks.mySortingList[i].myPlace == myStats.myPlace - 1)
            {
                //Switch!
                StartCoroutine(IERedShell(myRanks.mySortingList[i].gameObject));
            }
            else if (myStats.myPlace == 1)
            {
                StartCoroutine(IERedShell(myRanks.mySortingList[myRanks.mySortingList.Count].gameObject));

                //Destroy(gameObject);
            }
        }

    }

    IEnumerator IERedShell(GameObject _target)
    {
        yield return new WaitForEndOfFrame();
        navAgent.destination = _target.transform.position;

    }


    public void SetInstancer(GameObject _netId)
    {
        Rpc_SetMyObject(_netId.GetComponent<NetworkIdentity>().netId);
    }
    [Command]
    void Cmd_SetInstancer(NetworkInstanceId _netId)
    {
        //
        Debug.Log("Alone");

        instancer = NetworkServer.FindLocalObject(_netId).gameObject;
        Start2();
    }
    [ClientRpc]
    public void Rpc_SetMyObject(NetworkInstanceId _netId)
    {
        Debug.Log("Not Alone");

        instancer = ClientScene.FindLocalObject(_netId);
        Start2();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(IEAttack(collision.gameObject));
        }
    }

    IEnumerator IEAttack(GameObject myGo)
    {
        yield return new WaitForSeconds(0.1f);
        myGo.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
        gameObject.GetComponent<NavMeshAgent>().enabled = false;
        //Destroy(gameObject);
    }
}
