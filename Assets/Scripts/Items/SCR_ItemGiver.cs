using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_ItemGiver : MonoBehaviour {

	
    public void GiveItem(GameObject _player)
    {
        SCR_PlayerItems playerItems = _player.GetComponent<SCR_PlayerItems>();
        playerItems.myItem = (SCR_ItemManager.ItemIndex)Random.Range(0,playerItems.numItems-1);
        Destroy(gameObject);
    }
}
