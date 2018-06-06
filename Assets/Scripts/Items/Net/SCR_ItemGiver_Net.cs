﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_ItemGiver_Net : MonoBehaviour {

    public void GiveItem(GameObject _player)
    {
        SCR_PlayerItem_Net playerItems = _player.GetComponent<SCR_PlayerItem_Net>();
        playerItems.myItem = (SCR_ItemManager_Net.ItemIndex_Net)Random.Range(0, playerItems.numItems);
        Destroy(gameObject);
    }
}
