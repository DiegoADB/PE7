using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_PlayerItems : MonoBehaviour {

    SCR_ItemManager.ItemIndex myItem;
    GameObject currentItem;
    public int numItems = 0;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (currentItem == null)
        {

        }
        else
        {
            //Instantiate(currentItem, gameObject.transform).GetComponent
        }
    }

    public void SpawnCurrentItem()
    {
        switch (myItem)
        {
            case SCR_ItemManager.ItemIndex.SWITCHEROO:

                break;
        }
    }
}
