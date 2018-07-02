using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

//https://docs.unity3d.com/Manual/UNetLobby.html
public class CustomLobbyManager : NetworkLobbyManager
{
    [SerializeField] NetworkManager theNetworkManager;
	void Start ()
    {
        CStart();
        CListaSalas();
	}
	
    void CStart()
    {
        print("Se inicio");
        base.StartMatchMaker(); //INicializa
    }

    void CListaSalas()
    {
        print("Listas");
                                 //0 - 10, SIN FILTROS, Funcion que se llama al solicitar las salas
        base.matchMaker.ListMatches(0, 10, "", true, 0, 0, OnMatchList);
    }

    public override void OnMatchList(bool _sucess, string _extendInfo, List<UnityEngine.Networking.Match.MatchInfoSnapshot> _matchList)
    {
        print("OnMatchList");
        base.OnMatchList(_sucess, _extendInfo, _matchList);

        if (_sucess)
        {
            print("Numero de salas: " + _matchList.Count);
            if (_matchList.Count > 0)
            {
                bool createNewRoom = true;
                for (int i = 0; i < _matchList.Count; i++)
                {
                    if (_matchList[i].currentSize < 8)
                    {
                        CUnirSala(_matchList[i]);
                        createNewRoom = false;
                        break;
                    }
                }
                if(createNewRoom)
                    CCrearSala("Explosive Lobby " + _matchList.Count + 1);
                //print("Hay salas disponibles ");
                //print("Nombre de la sala: " + _matchList[0].name + " y su ID: " + _matchList[0].networkId);
                //CUnirSala(_matchList[0]); //Tratamos unirnos a la primera sala
            }
            else
            {
                //No hay salas creadas
                CCrearSala("Explosive Lobby 1"); //Intentamos creamos sala
            }
        }
        else
        {
            print("Error OnMatchList: " + _extendInfo);
        }
    }

    void CUnirSala(UnityEngine.Networking.Match.MatchInfoSnapshot _sala)
    {
        print("Unir a sala");
        base.matchMaker.JoinMatch(_sala.networkId, "", "", "", 0, 0, OnMatchJoined);
    }

    void CCrearSala(string _roomName)
    {
        print("Crear sala");   //NombreSala, Jugadores, EsPublica, contraseña
        base.matchMaker.CreateMatch(_roomName, 8, true, "", "", "", 0, 0, OnMatchCreate);
    }    

    public override void OnMatchJoined(bool _sucess, string _extendInfo, UnityEngine.Networking.Match.MatchInfo _matchInfo)
    {
        print("OnMatchJoined");
        base.OnMatchJoined(_sucess, _extendInfo, _matchInfo);

        print("Unido y estoy en la escena: " + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        if (_sucess)
        {
            print("Union exitosa" + _extendInfo);
        }
        else
        {
            print("Error al unir: " + _extendInfo);
        }
    }

    public override void OnLobbyServerPlayersReady()
    {
        print("OnLobbyServerPlayersReady");
        //base.OnLobbyServerPlayersReady();
        Invoke("IniciarPartida", 1.0f);
    }

    public void ShutDownNetworkManager(int _scene)
    {
        base.StopHost();
        base.StopMatchMaker();
        SceneManager.LoadScene(_scene);
    }

    public void IniciarPartida()
    {
		base.ServerChangeScene("hgfjgfhgf");
    }


    //Se llama cuando se genera el lobby player (SOLO EN SERVIDOR)
    public override GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId)
    {
        return base.OnLobbyServerCreateLobbyPlayer(conn, playerControllerId);
    }

    public override GameObject OnLobbyServerCreateGamePlayer(NetworkConnection conn, short playerControllerId)
    {
        print("OnLobbyServerCreateGamePlayer");
        GameObject obj = Instantiate(gamePlayerPrefab) as GameObject;
        return obj;
    }

    public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
    {
        Debug.Log("OnLobbyServerSceneLoadedForPlayer");

        NetworkConnection conn = lobbyPlayer.GetComponent<CustomLobbyPlayer>().connectionToClient;

        GameObject[] penguins = GameObject.FindGameObjectsWithTag("Player");
        GameObject tempPosition = GameObject.Find("Start Position " + (penguins.Length - 1));
        gamePlayer.transform.position = tempPosition.transform.position;
        gamePlayer.transform.rotation = tempPosition.transform.rotation;
        return base.OnLobbyServerSceneLoadedForPlayer(lobbyPlayer, gamePlayer);
    }

    /*public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        print("OnServerAddPlayer--------------------------------2");
        print("playerPrefab :" + playerPrefab);
        var player = (GameObject)GameObject.Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }*/





    /*public override void OnMatchCreate(bool _success, string _extendedInfo, UnityEngine.Networking.Match.MatchInfo _matchInfo)
    {
        print("OnMatchCreate");
        base.OnMatchCreate(_success, _extendedInfo, matchInfo);
        print("Sala creada ID: " + _matchInfo.networkId + " escena: " + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        if (_success)
        {
            print("Sala creada exitosamente: " + _extendedInfo);
        }
        else
        {
            print("Error al crear sala: " + _extendedInfo);
        }
    }*/


    /*public override void OnServerAddPlayer (NetworkConnection _conn, short _playerControllerId)
    {
    }*/

    /*public override void OnServerDisconnect (NetworkConnection _conn)
    {
    }*/

}
