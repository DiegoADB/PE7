using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Checkpoint : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<SCR_PlayerTempStats>().nextTarget==gameObject)
        {
            //AddnextTarget?
            other.GetComponent<SCR_PlayerTempStats>().myScore++;

        }
    }
}
