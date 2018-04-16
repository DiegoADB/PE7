using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_PlayerItems : MonoBehaviour {

    SCR_ItemManager myItemsManager;
    GameObject currentItem;
    public int numItems = 0;
    // Use this for initialization
    void Start()
    {
        myItemsManager = FindObjectOfType<SCR_ItemManager>();
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

    public void SetCurrentItem()
    {
        //currentItem = myItems[Random.Range(0, numItems)];
    }
}
