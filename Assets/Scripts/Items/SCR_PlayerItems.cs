using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_PlayerItems : MonoBehaviour {

    SCR_ItemManager itemManager;
    [SerializeField]
    SCR_ItemManager.ItemIndex myItem;
    GameObject currentItem;
    public int numItems = 0;
    // Use this for initialization
    void Start()
    {
        itemManager = GameObject.FindGameObjectWithTag("ItemManager").GetComponent<SCR_ItemManager>();
        myItem = SCR_ItemManager.ItemIndex.SWITCHEROO;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            SpawnCurrentItem();
        }


    }

    public void SpawnCurrentItem()
    {
        switch (myItem)
        {

            case SCR_ItemManager.ItemIndex.NONE:
                {

                }
                break;
            case SCR_ItemManager.ItemIndex.SWITCHEROO:
                {
                    Instantiate(itemManager.itemsList[(int)myItem]).GetComponent<SCR_Switcheroo>().SetInstancer(gameObject);
                    myItem = SCR_ItemManager.ItemIndex.NONE;
                }
                break;
            case SCR_ItemManager.ItemIndex.REDSHELL:
                {

                }
                break;
        }
    }
}
