using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_ItemRotation : MonoBehaviour {

	void Update ()
    {
        transform.Rotate(transform.up * 2);
	}
}
