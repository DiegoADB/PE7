using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_IgnoreParentRotation : MonoBehaviour {
	
	void Update ()
    {
        transform.rotation = Quaternion.identity;
	}
}
