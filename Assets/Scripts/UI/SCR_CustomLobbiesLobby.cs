using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class SCR_CustomLobbiesLobby : NetworkLobbyManager
{
    public Transform container;
    public GameObject lobbyRoomPrefab;
    [SerializeField] NetworkManager theNetworkManager;

    private void Start()
    {
        base.StartMatchMaker();
        LoadLobbyRooms();
    }

    public void LoadLobbyRooms()
    {
        foreach (Transform t in container)
        {
            Destroy(t.gameObject);
        }
        base.matchMaker.ListMatches(0, 10, "", true, 0, 0, OnMatchList);

    }

    public override void OnMatchList(bool _sucess, string _extendInfo, List<UnityEngine.Networking.Match.MatchInfoSnapshot> _matchList)
    {
        base.OnMatchList(_sucess, _extendInfo, _matchList);

        if (_sucess)
        {
            if (_matchList.Count > 0)
            {
                for (int i = 0; i < _matchList.Count; i++)
                {
                    GameObject tempLobby = Instantiate(lobbyRoomPrefab, container);
                    tempLobby.GetComponent<SCR_LobbyRoom>().SetUp("#" +(i).ToString(), _matchList[i].name, _matchList[i].currentSize.ToString() + "l8");
                    tempLobby.GetComponent<SCR_LobbyRoom>().room = i;
                    tempLobby.GetComponent<SCR_LobbyRoom>().join.onClick.AddListener(() => CUnirSala(_matchList[tempLobby.GetComponent<SCR_LobbyRoom>().room]));
                }
            }
        }
    }

    public void ShutDownNetworkManager(int _scene)
    {
        base.StopHost();
        base.StopMatchMaker();
        SceneManager.LoadScene(_scene);
    }

    public override void OnMatchJoined(bool _sucess, string _extendInfo, UnityEngine.Networking.Match.MatchInfo _matchInfo)
    {
        print("OnMatchJoined");
        base.OnMatchJoined(_sucess, _extendInfo, _matchInfo);

        if (_sucess)
        {
            SceneManager.LoadScene("Inicio");
        }
        else
        {
            print("Error al unir: " + _extendInfo);
        }
    }


    void CUnirSala(UnityEngine.Networking.Match.MatchInfoSnapshot _sala)
    {
        print("Unir a sala");
        base.matchMaker.JoinMatch(_sala.networkId, "", "", "", 0, 0, OnMatchJoined);
    }
}
