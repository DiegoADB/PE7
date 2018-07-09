using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SCR_PlayerItem_Net : NetworkBehaviour
{


    SCR_ItemManager_Net itemManager;
    [SerializeField]
    public SCR_ItemManager_Net.ItemIndex_Net myItem;
    [HideInInspector]
    public int numItems = 3;
    // Use this for initialization
    void Start()
    {
        enabled = base.isLocalPlayer;
        itemManager = GameObject.FindGameObjectWithTag("ItemManager").GetComponent<SCR_ItemManager_Net>();
        myItem = SCR_ItemManager_Net.ItemIndex_Net.NONE;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Cmd_SpawnCurrentItem();
        }
    }

    [Command]
    public void Cmd_SpawnCurrentItem()
    {
        switch (myItem)
        {

            case SCR_ItemManager_Net.ItemIndex_Net.NONE:
                {

                }
                break;
            case SCR_ItemManager_Net.ItemIndex_Net.SWITCHEROO:
                {
                    Debug.Log(gameObject.name);
                    GameObject item = Instantiate(itemManager.itemsList[(int)myItem]);
                    item.GetComponent<SCR_Switcheroo>().Cmd_SetInstancer(gameObject);
                    NetworkServer.Spawn(item);
                    myItem = SCR_ItemManager_Net.ItemIndex_Net.NONE;
                }
                break;
            case SCR_ItemManager_Net.ItemIndex_Net.REDSHELL:
                {
                    GameObject item = Instantiate(itemManager.itemsList[(int)myItem], transform.position, transform.rotation);
                    item.GetComponent<SCR_RedShell_Net>().Rpc_SetInstancer(gameObject);
                    NetworkServer.Spawn(item);
                    myItem = SCR_ItemManager_Net.ItemIndex_Net.NONE;
                }
                break;
            case SCR_ItemManager_Net.ItemIndex_Net.ORCA:
                {
                    GameObject item = Instantiate(itemManager.itemsList[(int)myItem], transform.position, transform.rotation);
                    item.GetComponent<SCR_OrcaBill>().Rpc_SetInstancer(gameObject);
                    NetworkServer.Spawn(item);
                    myItem = SCR_ItemManager_Net.ItemIndex_Net.NONE;
                }
                break;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ItemBirth"))
        {
            Debug.Log("Hewwo");
            if (myItem == SCR_ItemManager_Net.ItemIndex_Net.NONE)
            {
                other.GetComponent<SCR_ItemGiver_Net>().GiveItem(gameObject);
                Debug.Log("Item: " + myItem.ToString());
            }
            else
            {
                Debug.Log("Im Full");

            }

        }
    }
}
