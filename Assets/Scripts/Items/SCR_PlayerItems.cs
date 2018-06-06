using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_PlayerItems : MonoBehaviour {

    
    SCR_ItemManager itemManager;
    [SerializeField]
    public SCR_ItemManager.ItemIndex myItem;
    [HideInInspector]
    public int numItems = 3;
    // Use this for initialization
    void Start()
    {
        itemManager = GameObject.FindGameObjectWithTag("ItemManager").GetComponent<SCR_ItemManager>();
        myItem = SCR_ItemManager.ItemIndex.NONE;

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
                    Instantiate(itemManager.itemsList[(int)myItem],transform.position,transform.rotation).GetComponent<SCR_RedShell>().SetInstancer(gameObject);
                    myItem = SCR_ItemManager.ItemIndex.NONE;
                }
                break;
            case SCR_ItemManager.ItemIndex.ORCA:
                {
                    Instantiate(itemManager.itemsList[(int)myItem],transform.position,transform.rotation).GetComponent<SCR_OrcaBill>().SetInstancer(gameObject);
                    myItem = SCR_ItemManager.ItemIndex.NONE;
                }
                break;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("ItemBirth"))
        {
            if(myItem==SCR_ItemManager.ItemIndex.NONE)
            {
                other.GetComponent<SCR_ItemGiver>().GiveItem(gameObject);
                Debug.Log("Item: "+myItem.ToString());
            }
            else
            {
                Debug.Log("Im Full");

            }

        }
    }
}
