using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class SCR_Disconnect : MonoBehaviour
{
    public static void DisconnectFromMatch()
    {
        NetworkManager manager = NetworkManager.singleton;
        MatchInfo match = manager.matchInfo;
        manager.matchMaker.DropConnection(match.networkId, match.nodeId, 0, manager.OnDropConnection);
        manager.StopHost();
        SceneManager.LoadScene("Menu");
    }
}
