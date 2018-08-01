using UnityEngine;
using UnityEngine.UI;

public class SCR_CurrentItemUI : MonoBehaviour
{
    private Image currentItem;
    public Sprite[] items;
    SCR_PlayerItem_Net myItem;

    private void Start()
    {
        currentItem = GetComponent<Image>();
        myItem = transform.root.GetComponent<SCR_PlayerItem_Net>();
    }

    private void Update()
    {
        currentItem.sprite = items[(int)myItem.myItem + 1];
    }

}
