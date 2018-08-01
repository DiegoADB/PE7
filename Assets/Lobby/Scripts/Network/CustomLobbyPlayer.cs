using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CustomLobbyPlayer : NetworkLobbyPlayer
{
    public Toggle tgl_ready;
    public Text txt_Jugador;
    public Image background;
    private bool imReady = false;
    public override void OnClientEnterLobby()
    {
        print("OnClientEnterLobby");
        transform.SetParent(GameObject.Find("PlayerLobby").transform, false);
        //agregamos listner al UI
        tgl_ready.onValueChanged.AddListener(CambioReady);
        //actualizamos nombre de jugador
        txt_Jugador.text = "Player " + (base.slot + 1).ToString();
        OnClientReady(false);
        if (NetworkServer.connections.Count == 0)
        {
            GameObject temp = GameObject.Find("Start Game");
            if(temp!=null)
                temp.SetActive(false);
        }
            
    }

    public override void OnClientExitLobby()
    {
        print("OnClientExitLobby");
    }

    private void Update()
    {
        if (Input.GetButtonDown("P1_Start"))
            CambioReady(true);
    }

    public override void OnClientReady(bool readyState)
    {
        print("OnClientReady");
        //actaulizamos la version local representativo
        tgl_ready.isOn = readyState;
        if(readyState)
        {
            Text container = Instantiate(txt_Jugador, GameObject.Find("PlayerLobby").transform);
            container.text = "";
            Image tempBg = Instantiate(background, container.transform);
            Toggle temp = Instantiate(tgl_ready, container.transform);
            temp.interactable = false;
            Text tempText = Instantiate(txt_Jugador, container.transform);
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
    }

    public override void OnStartClient()
    {
        //All networkbehaviour base function don't do anything
        //but NetworkLobbyPlayer redefine OnStartClient, so we need to call it here
        base.OnStartClient();
        print("OnStartClient");
        //setup the player data on UI. The value are SyncVar so the player
        //will be created with the right value currently on server
    }

    public override void OnStartLocalPlayer()
    {
        print("OnStartLocalPlayer");
        //activamos solo la version local
        tgl_ready.interactable = true;
        base.OnStartLocalPlayer();
    }

    public void CambioReady(bool _set)
    {
        if (imReady)
            return;
        //cambiamos para acvisarle a los demas, pero solo si es local
        if (!base.isLocalPlayer) return;

        if (_set)
            base.SendReadyToBeginMessage();
        else
            base.SendNotReadyToBeginMessage();
        imReady = true;
    }
}
