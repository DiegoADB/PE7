using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_UIAnimation : MonoBehaviour
{

    public Transform iceMax;
    public Transform iceMin;
    public GameObject iceBerg;

    private Vector3 maxPosition;
    private Vector3 minPosition;

    private bool b_up;
    private bool b_down;

	void Start ()
    {
        b_up = true;
        b_down = false;
        maxPosition = iceMax.position;
        minPosition = iceMin.position;

        StartCoroutine(BobbleIce());
	}
	
	//void Update ()
 //   {
		
	//}


    IEnumerator BobbleIce()
    {
        if(b_up && !b_down)
            iceBerg.transform.position = Vector3.Lerp(iceBerg.transform.position, maxPosition, 0.3f * Time.deltaTime);

        if(b_down && !b_up)
            iceBerg.transform.position = Vector3.Lerp(iceBerg.transform.position, minPosition, 0.3f * Time.deltaTime);


        if (iceBerg.transform.position.y >= maxPosition.y - 0.09f)
        {
            Debug.Log("Reached MAX");
            b_up = false;
            b_down = true;
        }

        if(iceBerg.transform.position.y <= minPosition.y + 0.09f)
        {
            Debug.Log("Reached MIN");
            b_up = true;
            b_down = false;
        }

        yield return new WaitForEndOfFrame();

        StartCoroutine(BobbleIce());
    }

}
