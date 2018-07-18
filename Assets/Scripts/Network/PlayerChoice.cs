using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerChoice : NetworkBehaviour
{
    [SyncVar] public int choice;
    private void Start()
    {
        enabled = isLocalPlayer;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
            choice = 0;
        if (Input.GetKeyDown(KeyCode.Alpha1))
            choice = 1;
        if (Input.GetKeyDown(KeyCode.Alpha2))
            choice = 2;
        if (Input.GetKeyDown(KeyCode.Alpha3))
            choice = 3;
    }
    //    public void RegisterPlayer(int _id, int _type)
    //    {
    //        if (!thePlayers.ContainsKey(_id))
    //            thePlayers.Add(_id, _type);
    //        else
    //            thePlayers[_id] = _type;
    //    }
}
