using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class SCR_ItemGiver_Net : NetworkBehaviour {

    [Command]
    public void Cmd_GiveItem(GameObject _player)
    {
        
        gameObject.GetComponent<SphereCollider>().enabled = false;
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        Invoke("Reactivate", 5);

    }
    void Reactivate()
    {
        gameObject.GetComponent<SphereCollider>().enabled = true;
        gameObject.transform.GetChild(0).gameObject.SetActive(true);


    }
}
