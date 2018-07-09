using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_PlayerItem_Net : MonoBehaviour
{


    SCR_ItemManager_Net itemManager;
    [SerializeField]
    public SCR_ItemManager_Net.ItemIndex_Net myItem;
    [HideInInspector]
    public int numItems = 3;
    // Use this for initialization
    void Start()
    {
        itemManager = GameObject.FindGameObjectWithTag("ItemManager").GetComponent<SCR_ItemManager_Net>();
        myItem = SCR_ItemManager_Net.ItemIndex_Net.NONE;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            SpawnCurrentItem();
        }


    }

    public void SpawnCurrentItem()
    {
        switch (myItem)
        {

            case SCR_ItemManager_Net.ItemIndex_Net.NONE:
                {

                }
                break;
            case SCR_ItemManager_Net.ItemIndex_Net.SWITCHEROO:
                {
                    Instantiate(itemManager.itemsList[(int)myItem]).GetComponent<SCR_Switcheroo>().SetInstancer(gameObject);
                    myItem = SCR_ItemManager_Net.ItemIndex_Net.NONE;
                }
                break;
            case SCR_ItemManager_Net.ItemIndex_Net.REDSHELL:
                {
                    Instantiate(itemManager.itemsList[(int)myItem], transform.position, transform.rotation).GetComponent<SCR_RedShell_Net>().SetInstancer(gameObject);
                    myItem = SCR_ItemManager_Net.ItemIndex_Net.NONE;
                }
                break;
            case SCR_ItemManager_Net.ItemIndex_Net.ORCA:
                {
                    Instantiate(itemManager.itemsList[(int)myItem], transform.position, transform.rotation).GetComponent<SCR_OrcaBill>().SetInstancer(gameObject);
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
