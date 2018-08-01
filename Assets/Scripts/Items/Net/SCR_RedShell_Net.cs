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
    public GameObject myTarget;
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
                Debug.Log("RedShell Not First");
                myTarget=myRanks.mySortingList[i].gameObject;


            }
            else if (myStats.myPlace == 1)
            {
                Debug.Log("RedShell Not First");

                myTarget = myRanks.mySortingList[myRanks.mySortingList.Count-1].gameObject;

            }
        }

    }
    [ClientRpc]
    void Rpc_FollowEnemy()
    {
        Debug.Log("RedShellTarget:" + myTarget.name);
        StartCoroutine(IERedShell());

    }
    void Update()
    {
        if(navAgent.destination!=null)
        {
            navAgent.SetDestination(myTarget.transform.position);
            Debug.Log("Navmeshagent" + navAgent.destination);

        }
    }
    IEnumerator IERedShell()
    {
        yield return new WaitForEndOfFrame();
        navAgent.SetDestination(myTarget.transform.position);
        Debug.Log("Navmeshagent" + navAgent.destination);

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
    [ServerCallback]
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player" && collision.gameObject !=instancer)
        {
            //StartCoroutine(IEAttack(collision.gameObject));
            collision.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            collision.gameObject.GetComponent<SCR_CharacterMotor>().currentSpeed = 0;

            gameObject.GetComponent<NavMeshAgent>().enabled = false;
            Destroy(gameObject);
        }
    }

    IEnumerator IEAttack(GameObject myGo)
    {
        yield return new WaitForSeconds(0.1f);
        myGo.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
        myGo.GetComponent<SCR_CharacterMotor>().currentSpeed = 0;

        gameObject.GetComponent<NavMeshAgent>().enabled = false;
        Destroy(gameObject);
    }
}
