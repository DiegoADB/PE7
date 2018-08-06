using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class SCR_Switcheroo : NetworkBehaviour {

    public GameObject instancer;
    SCR_PlayerTempStats myStats;
    SCR_Ranking myRanks;
    public float timeBeforeChange = 1;

	public void Start2 () {
        transform.parent = instancer.transform;
        transform.localPosition = new Vector3(0, 0.5f, 0);
        myStats = instancer.GetComponent<SCR_PlayerTempStats>();
        myRanks = GameObject.FindGameObjectWithTag("RankingManager").GetComponent<SCR_Ranking>();
        for (int i = 0; i < myRanks.playerNum; i++)
        {
            if(myRanks.mySortingList[i].myPlace == myStats.myPlace-1)
            {
                StartCoroutine(IESwitcheroo(myRanks.mySortingList[i].gameObject));
            }
            else if(myStats.myPlace ==1)
            {
                NetworkServer.Destroy(gameObject);
            }
        }

    }
    IEnumerator IESwitcheroo(GameObject _target)
    {
        Vector3 otherPos = _target.transform.position;
        Quaternion otherRot = _target.transform.rotation;
        yield return new WaitForSeconds(timeBeforeChange);
        _target.transform.position = instancer.transform.position - (instancer.transform.forward * 2);
        _target.transform.rotation = instancer.transform.rotation;
        Destroy(gameObject);
    }
    public void SetInstancer(GameObject _netId)
    {

        Rpc_SetMyObject(_netId.GetComponent<NetworkIdentity>().netId);
    }
    [Command]
    void Cmd_SetInstancer(NetworkInstanceId _netId)
    {
        instancer = NetworkServer.FindLocalObject(_netId).gameObject;
        Start2();
    }
    [ClientRpc]
    public void Rpc_SetMyObject(NetworkInstanceId _netId)
    {
        instancer = ClientScene.FindLocalObject(_netId);
        Start2();
    }
}
