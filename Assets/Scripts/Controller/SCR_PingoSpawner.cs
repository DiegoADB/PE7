using UnityEngine;
using UnityEngine.Networking;

public class SCR_PingoSpawner : NetworkBehaviour
{
    public string pingoName;
    private void Start()
    {
        enabled = isLocalPlayer;
        Cmd_ChangePlayer();
    }

    [Command]
    void Cmd_ChangePlayer()
    {
        var conn = GetComponent<NetworkIdentity>().connectionToClient;
        var newPlayer = Instantiate<GameObject>(Resources.Load<GameObject>("Pingos/" + pingoName), transform.position, transform.rotation);
        Destroy(GetComponent<NetworkIdentity>().gameObject);
        NetworkServer.ReplacePlayerForConnection(conn, newPlayer, 0);
        Rpc_SpawnPlayer();
    }

    [ClientRpc]
    void Rpc_SpawnPlayer()
    {
        var conn = GetComponent<NetworkIdentity>().connectionToClient;
        var newPlayer = Instantiate<GameObject>(Resources.Load<GameObject>("Pingos/" + pingoName), transform.position, transform.rotation);
        Destroy(GetComponent<NetworkIdentity>().gameObject);
        NetworkServer.ReplacePlayerForConnection(conn, newPlayer, 0);
    }

}
