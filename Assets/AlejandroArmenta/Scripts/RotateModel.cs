using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateModel : MonoBehaviour {

	// Use this for initialization
	Transform ModelTransform;
	void Start () {
		ModelTransform = GetComponent<Transform> (); 
	}
	
	// Update is called once per frame
	void Update () {
		ModelTransform.RotateAround (ModelTransform.position, Vector3.up, 20 * Time.deltaTime);
	}
}
