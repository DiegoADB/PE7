using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class SCR_Switcheroo : NetworkBehaviour {

    public GameObject instancer;
    SCR_PlayerTempStats myStats;
    SCR_Ranking myRanks;
    public float timeBeforeChange = 1.5f;

	// Use this for initialization
	public void Start2 () {
        //SetInstancer();
        transform.parent = instancer.transform;
        transform.localPosition = new Vector3(0, 0.5f, 0);
        myStats = instancer.GetComponent<SCR_PlayerTempStats>();
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
        _target.transform.position = instancer.transform.position;
        //Could be removed
        _target.transform.rotation = instancer.transform.rotation;
        instancer.transform.rotation = otherRot;
        instancer.transform.position = otherPos;
        Destroy(gameObject);
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
