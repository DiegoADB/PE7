using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using UnityEngine.Networking;

public class SCR_OrcaBill : NetworkBehaviour {

    private NavMeshAgent _navAgnt;
    Transform pingo;
    int myScore = -1;
    public GameObject instancer;

    SCR_PlayerTempStats next_dest;
    public int totalchk;
    // Use this for initialization
    public void Start2()
    {
        //SetInstancer();
        _navAgnt = gameObject.GetComponent<NavMeshAgent>();
        pingo = instancer.GetComponent<Transform>();
        next_dest = instancer.GetComponent<SCR_PlayerTempStats>();
        pingo.SetParent(transform);
        instancer.transform.GetChild(0).gameObject.SetActive(false);
        instancer.GetComponent<SCR_CharacterMotor_Net>().orca = true;
        instancer.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX  | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

    }

    // Update is called once per frame
    void Update()
    {
        if (!instancer)
            return;
        if (_navAgnt.enabled == true)
        {
            nav_mech();
        }
        //Agregar Follow de camera(SCR_CharacterMotor)
        if (myScore + totalchk == next_dest.myScore)
        {
            //_navAgnt.Warp(new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z));
            instancer.transform.GetChild(0).gameObject.SetActive(true);
            instancer.GetComponent<SCR_CharacterMotor_Net>().enabled = true;
            instancer.GetComponent<Rigidbody>().constraints =  RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            

            _navAgnt.isStopped = true;
            pingo.parent = null;
            instancer.GetComponent<SCR_CharacterMotor_Net>().orca = false;
            if(pingo.parent==null)
                Destroy(this.gameObject);   
        }
    }

    void nav_mech()
    {
        if (myScore == -1)
        {
            myScore = next_dest.myScore;
        }
        _navAgnt.destination = next_dest.nextTarget.transform.position;
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