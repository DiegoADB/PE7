using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public struct MyEvent : UnityEvent<int>
{

}
public class DisplayControl : MonoBehaviour {

	void Assert(bool Expression)
	{
		if(!(Expression))
		{
			throw new System.ArgumentException("Parameter cannot be null", "original");
			//System.AccessViolationException;
		}
	}

	public MyEvent Event;

	struct custom_elements
	{
		public float Velocity;
		/*public float Control;
		public float Force;
		public float HP;*/
	};

	static custom_elements [] CustomElements;

	//public Text MyText;
	public RectTransform [] Rects;

	public Button [] Buttons;
	/*
	float RectDimX;
	float RectDimY;
	*/
	Component [] c;

	void Start () {
		int ElementIndex = 0;
		CustomElements[ElementIndex++].Velocity = 100.0f;	
		CustomElements[ElementIndex++].Velocity = 200.0f;	
		CustomElements[ElementIndex++].Velocity = 250.0f;	

		Assert(ElementIndex <= CustomElements.Length);

		Assert(CustomElements.Length);

		//Buttons[0].onClick.
		Event.AddListener(ModifyStat);
		//Buttons[1].onClick.
		Event.AddListener(ModifyStat);
		/*
		Buttons[2].onClick.AddListener(ModifyForce);
		Buttons[3].onClick.AddListener(ModifyHP);

		Buttons[2].onClick.AddListener(SelectImage);
		Buttons[3].onClick.AddListener(SelectImage);

		//CustomElement = new custom_elements ();	
		//CustomElement.Velocity = 0.5f; 
		//CustomElement.Control = 0.5f; 
		//CustomElement.Force = 0.5f; 
		//CustomElement.Control = 0.5f; 

		RectDimX = Rects[0].sizeDelta.x;
		RectDimY = Rects[0].sizeDelta.y;

		RectDimX = Rects[0].sizeDelta.x;
		RectDimY = Rects[0].sizeDelta.y;

		RectDimX = Rects[0].sizeDelta.x;
		RectDimY = Rects[0].sizeDelta.y;
		*/
	}
		
	void ModifyStat(int ElementIndex)
	{
		StartCoroutine (Draw (Rects[0], ElementIndex));
	}

	/*
	void ModifyVelocity()
	{
		//TODO: Check what happens when calling it multiple times
		StartCoroutine (Draw (Rects[0], 100));
	}

	void ModifyControl()
	{
		//TODO: Check what happens when calling it multiple times
		StartCoroutine (Draw (Rects[1], 100));
	}

	void ModifyForce()
	{
		//TODO: Check what happens when calling it multiple times
		StartCoroutine (Draw (Rects[2], 100));
	}

	void ModifyHP()
	{
		//TODO: Check what happens when calling it multiple times
		StartCoroutine (Draw (Rects[3], 100));
	}
	*/

	//public float Duration; //NOTE:Seconds

	public float 
	Abs(float Value)
	{
		float Result = (Value < 0) ? -Value : Value;
		return Result;
	}

	public float CurrentSize;

	IEnumerator 
	Draw(UnityEngine.RectTransform rect, float DestSize)
	{
		Assert (CurrentSize != DestSize);
		float DeltaP = DestSize - CurrentSize;
		float Sign = DeltaP / Abs(DeltaP);

		float RectDimY = rect.sizeDelta.y;

		float t = CurrentSize;
		float RecVel = .001f; //m / s;
		float BaseTimeInSeconds = Time.realtimeSinceStartup;
		while(t != DestSize)
		{
			rect.sizeDelta = new Vector2 (t, RectDimY);

			//Debug.Log ("X: " + VelocityRect.sizeDelta.x.ToString() + "Y: " + VelocityRect.sizeDelta.y.ToString());
			//Debug.Log ("Time: " + Time.unscaledTime.ToString());
			yield return new WaitForEndOfFrame();

			float DeltaT = Time.realtimeSinceStartup - BaseTimeInSeconds;
			t += DeltaT * RecVel * Sign;
			Debug.Log ("BaseTime: " + DeltaT.ToString());

			//NOTE: Try DeltaTime 

		}
	}

	public void OnPointerDown(PointerEventData Data)
	{
		//So bizarre!!
	}
	// Update is called once per frame
	void Update () {
		
	}


}
