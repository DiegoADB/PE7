using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_ItemManager_Net : MonoBehaviour {

    public GameObject[] itemsList;

    public enum ItemIndex_Net
    {
        NONE = -1,
        SWITCHEROO,
        REDSHELL,
        ORCA,
        BOOST
    }
}
