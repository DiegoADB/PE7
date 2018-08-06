using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;
public class SCR_RedShell_Net : NetworkBehaviour {

    public GameObject instancer;
    SCR_PlayerTempStats myStats;
    SCR_Ranking myRanks;
    public GameObject myTarget;
    float timer = 0;
    public void Start2()
    {
        myStats = instancer.GetComponent<SCR_PlayerTempStats>();
        myRanks = GameObject.FindGameObjectWithTag("RankingManager").GetComponent<SCR_Ranking>();
        transform.position = new Vector3(instancer.transform.position.x, instancer.transform.position.y, instancer.transform.position.z) + gameObject.transform.forward;
        for (int i = 0; i < myRanks.playerNum; i++)
        {
            if (myRanks.mySortingList[i].myPlace == myStats.myPlace - 1)
            {
                myTarget=myRanks.mySortingList[i].gameObject;
            }
            else if (myStats.myPlace == 1)
            {
                myTarget = myRanks.mySortingList[myRanks.mySortingList.Count-1].gameObject;
                NetworkServer.Destroy(gameObject);
            }
        }

    }
    [ServerCallback]
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, myTarget.transform.position, Time.deltaTime * 30);
        transform.rotation = Quaternion.LookRotation(myTarget.transform.position - transform.position);
        timer += Time.deltaTime;
        if (timer > 5.0f)
            NetworkServer.Destroy(gameObject);
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
}
