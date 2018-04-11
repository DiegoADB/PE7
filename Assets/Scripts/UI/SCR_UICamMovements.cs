using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_UICamMovements : MonoBehaviour {

    public Transform mainTransform;
    public Transform optionsTransform;


    public void OnOptionsClicked()
    {
        Debug.Log("I HAVE BEEN CLICKED");
        StartCoroutine(MoveCamera(optionsTransform));
    }

    public void OnBackToMainClicked()
    {
        StartCoroutine(MoveCamera(mainTransform));
    }

    IEnumerator MoveCamera(Transform _endPos)
    {
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, _endPos.position, 5.0f * Time.deltaTime);
        Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, _endPos.rotation, 5.0f * Time.deltaTime);
        yield return new WaitForSeconds(0.05f);
        StartCoroutine(MoveCamera(_endPos));
    }
}
