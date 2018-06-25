using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCR_LobbyRoom : MonoBehaviour
{
    public Text number;
    public Text theName;
    public Text players;
    public Button join;
    public int room;


    public void SetUp(string _num, string _name, string _playerCount)
    {
        number.text = _num;
        theName.text = _name;
        players.text = _playerCount;
    }
	
}
