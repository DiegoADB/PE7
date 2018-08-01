using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using UnityEngine.Networking;

public class SCR_OrcaBill : NetworkBehaviour {

    Transform pingo;
    int myScore = -1;
    public GameObject instancer;

    SCR_PlayerTempStats next_dest;
    public int totalchk;
    // Use this for initialization
    public void Start2()
    {
        //SetInstancer();
        pingo = instancer.GetComponent<Transform>();
        next_dest = instancer.GetComponent<SCR_PlayerTempStats>();
        pingo.SetParent(transform);
        instancer.transform.GetChild(0).gameObject.SetActive(false);
        instancer.GetComponent<SCR_CharacterMotor_Net>().orca = true;
        instancer.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX  | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        if (!instancer)
            return;
        if (Vector3.Distance(transform.position, next_dest.nextTarget.transform.position) > 5)
        {
            transform.position = Vector3.MoveTowards(transform.position, next_dest.nextTarget.transform.position, Time.deltaTime * 20);
            transform.rotation = Quaternion.LookRotation(next_dest.nextTarget.transform.position - transform.position);
        }
        else
        {
            instancer.transform.GetChild(0).gameObject.SetActive(true);
            instancer.GetComponent<SCR_CharacterMotor_Net>().enabled = true;
            instancer.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;


            //_navAgnt.isStopped = true;
            pingo.parent = null;
            instancer.GetComponent<SCR_CharacterMotor_Net>().orca = false;
            if (pingo.parent == null)
                Destroy(this.gameObject);
        }
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