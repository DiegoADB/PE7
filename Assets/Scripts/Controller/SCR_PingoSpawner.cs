using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class SCR_PingoSpawner : NetworkBehaviour
{
    private void Start()
    {
        enabled = isLocalPlayer;
        Cmd_ChangePlayer();
    }

    [Command]
    void Cmd_ChangePlayer()
    {
        var conn = GetComponent<NetworkIdentity>().connectionToClient;
        var newPlayer = Instantiate<GameObject>(Resources.Load<GameObject>("Pingos/Lucky"), transform.position, transform.rotation);
        Destroy(GetComponent<NetworkIdentity>().gameObject);
        NetworkServer.ReplacePlayerForConnection(conn, newPlayer, 0);
        //Rpc_SpawnPlayer();
    }

    [ClientRpc]
    void Rpc_SpawnPlayer()
    {
        var conn = GetComponent<NetworkIdentity>().connectionToClient;
        var newPlayer = Instantiate<GameObject>(Resources.Load<GameObject>("Pingos/Normal"), transform.position, transform.rotation);
        Destroy(GetComponent<NetworkIdentity>().gameObject);
        NetworkServer.ReplacePlayerForConnection(conn, newPlayer, 0);
    }

}
