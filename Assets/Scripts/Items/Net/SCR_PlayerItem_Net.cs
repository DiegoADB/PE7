using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SCR_PlayerItem_Net : NetworkBehaviour
{


    SCR_ItemManager_Net itemManager;
    [SyncVar]
    public SCR_ItemManager_Net.ItemIndex_Net myItem;
    public int numItems = 4;
    // Use this for initialization
    void Start()
    {
        enabled = base.isLocalPlayer;
        itemManager = GameObject.FindGameObjectWithTag("ItemManager").GetComponent<SCR_ItemManager_Net>();
        myItem = SCR_ItemManager_Net.ItemIndex_Net.NONE;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) || Input.GetButtonDown("P1_X"))
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
                    GameObject item = Instantiate(itemManager.itemsList[(int)myItem]);
                    NetworkServer.Spawn(item);
                    item.GetComponent<SCR_Switcheroo>().SetInstancer(gameObject);

                    myItem = SCR_ItemManager_Net.ItemIndex_Net.NONE;
                }
                break;
            case SCR_ItemManager_Net.ItemIndex_Net.REDSHELL:
                {
                    GameObject item = Instantiate(itemManager.itemsList[(int)myItem], transform.position, transform.rotation);
                    NetworkServer.Spawn(item);
                    item.GetComponent<SCR_RedShell_Net>().SetInstancer(gameObject);
                    myItem = SCR_ItemManager_Net.ItemIndex_Net.NONE;
                }
                break;
            case SCR_ItemManager_Net.ItemIndex_Net.ORCA:
                {
                    GameObject item = Instantiate(itemManager.itemsList[(int)myItem], transform.position, transform.rotation);
                    NetworkServer.Spawn(item);
                    item.GetComponent<SCR_OrcaBill>().SetInstancer(gameObject);

                    myItem = SCR_ItemManager_Net.ItemIndex_Net.NONE;
                }
                break;
            case SCR_ItemManager_Net.ItemIndex_Net.BOOST:
                {
                    GetComponent<SCR_CharacterMotor_Net>().Rpc_Boost();
                    myItem = SCR_ItemManager_Net.ItemIndex_Net.NONE;
                }
                break;
        }
        Debug.Log("Item Value " + (int)myItem);

    }

    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ItemBirth"))
        {
            if (myItem == SCR_ItemManager_Net.ItemIndex_Net.NONE)
            {
                other.GetComponent<SCR_ItemGiver_Net>().Rpc_Deactivate();
                Cmd_GiveItem();
            }
        }
    }
    [Command]
    void Cmd_GiveItem()
    {
            myItem = (SCR_ItemManager_Net.ItemIndex_Net)Random.Range(0, numItems);
    }
}
