using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DisplayControl : MonoBehaviour {

	void Assert(bool Expression)
	{
		if(!(Expression))
		{
			throw new System.ArgumentException("Parameter cannot be null", "original");
			//System.AccessViolationException;
		}
	}
	/*
	float max;
	float min;

	struct custom_elements
	{
		public float Velocity;
		public float Control;
		public float Force;
		public float HP;
	};
	//static custom_elements CustomElement;
*/
	//public Text MyText;
	public RectTransform [] Rects;

	public Button [] Buttons;
	/*
	float RectDimX;
	float RectDimY;
	*/
	Component [] c;

	void Start () {
		//Text VelocityText = MyText.GetComponent<Text> ();

		Buttons[0].onClick.AddListener(ModifyVelocity);
		Buttons[1].onClick.AddListener(ModifyControl);
		Buttons[2].onClick.AddListener(ModifyForce);
		Buttons[3].onClick.AddListener(ModifyHP);

		/*
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


	public float Duration; //NOTE:Seconds

	IEnumerator Draw(UnityEngine.RectTransform rect, float EndSize)
	{
		float RectDimX = rect.sizeDelta.x;
		float RectDimY = rect.sizeDelta.y;

		Assert (RectDimX > EndSize);
		//NOTE: Set the InitSize as the one set by the user
		float InitSize = RectDimX;

		float t = 0.0f;

		float Proportion = 1.0f / Duration;
		float BaseTimeInSeconds = Time.realtimeSinceStartup;
		while(t < 1.0f)
		{
			//NOTE: Parameter size 
			float Value = (1.0f - t) * InitSize + t * EndSize;

			rect.sizeDelta = new Vector2 (Value, RectDimY);
			//yield return new WaitForEndOfFrame (2);
			//Debug.Log ("X: " + VelocityRect.sizeDelta.x.ToString() + "Y: " + VelocityRect.sizeDelta.y.ToString());
			Debug.Log ("Time: " + Time.unscaledTime.ToString());
			//Debug.Log ("BaseTime: " + BaseTimeInSeconds.ToString());
			yield return new WaitForEndOfFrame();

			t = (Time.realtimeSinceStartup - BaseTimeInSeconds) * Proportion;

		}
	}

	// Update is called once per frame
	void Update () {
		
	}


}
