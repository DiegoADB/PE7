using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetItemPanel_ : MonoBehaviour 
{

	RectTransform ItemPanel;
	DisplayControl display;

	int ItemCount;

	RectTransform [] ButtonRects;

	void Start () {
		ItemPanel = GetComponent<RectTransform> ();
		Component [] comp = GetComponentsInChildren<RectTransform> ();
		Vector2 At = new Vector2 (ItemPanel.rect.xMin, ItemPanel.rect.yMin);
		Vector2 MaxDim = new Vector2 (ItemPanel.sizeDelta.x, 20.0f);
		Debug.Log ("MaxDim X: " + MaxDim.x.ToString() + 
			"MaxDim Y: " + MaxDim.y.ToString());

		float YSpacing = 5.0f;
		//NOTE: Size delta
		foreach (RectTransform rect in comp) 
		{

			At = At + (new Vector2 (0, MaxDim.y + YSpacing));
			rect.sizeDelta = MaxDim;
			rect.offsetMin = At;
		}
	}

	#if false
	void SetRect(Vector2 At, Vector2 Dim)
	{
		
	}
	#endif
	// Update is called once per frame
	void Update () {
		
	}
}
