using UnityEngine;
using UnityEngine.EventSystems;

public class InputTest : MonoBehaviour
{
    [HideInInspector]
    public int ItemIndex;
    Stats StatRef;

    void Start()
    {
        StatRef = FindObjectOfType<Stats>();
    }

#if false
    public void OnPointerClick(PointerEventData pointerEventData)
	{
		if(pointerEventData.button == PointerEventData.InputButton.Left)
		{
			//NOTE(Alex): Left mouse clicked.
			Debug.Log("Left Mouse clicked");
            StatRef.DrawItemStats(ItemIndex);
		}
	}
#else

    public void SwithColumns(int Sign)
    {
        StatRef.SwitchColumns(ref StatRef.CState,  Sign);
    }

    public void SwitchRows(int Sign)
    {
        StatRef.SwitchRows(ref StatRef.CState, Sign);
    }

    public void ElementClicked()
    {
        //NOTE(Alex): Left mouse clicked.
        Debug.Log("Left Mouse clicked");
        StatRef.DrawItemStats(ItemIndex);
    }
#endif
}
