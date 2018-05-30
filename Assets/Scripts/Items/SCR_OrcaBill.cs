using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class SCR_OrcaBill : MonoBehaviour {
    private NavMeshAgent _navAgnt;
    Transform pingo;
    int myScore = -1;
    GameObject instancer;

    SCR_PlayerTempStats next_dest;
    public int totalchk;
    // Use this for initialization
    void Start()
    {
        
        _navAgnt = gameObject.GetComponent<NavMeshAgent>();
        pingo = instancer.GetComponent<Transform>();
        next_dest = instancer.GetComponent<SCR_PlayerTempStats>();
        pingo.SetParent(transform);
        instancer.transform.GetChild(0).gameObject.SetActive(false);
        instancer.GetComponent<SCR_CharacterMotor_Net>().enabled = false;
        instancer.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX  | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

    }

    // Update is called once per frame
    void Update()
    {
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

    public  void SetInstancer(GameObject _instancer)
    {
        instancer = _instancer;
    }
}