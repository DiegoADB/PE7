using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_UICamMovements : MonoBehaviour {

    public Transform mainTransform;
    public Transform optionsTransform;

    private void Start()
    {
        transform.position = mainTransform.position;
        transform.rotation = mainTransform.rotation;
    }

    private void Update()
    {
        Debug.Log("camera: " + transform.localRotation);
        Debug.Log("mainT: " + mainTransform.localRotation);
    }

    public void OnOptionsClicked()
    {
        //StartCoroutine(MoveCamera(optionsTransform));
        MoveCameraTween(optionsTransform);
    }

    public void MoveCameraTween(Transform _endpos)
    {
        iTween.MoveTo(gameObject, _endpos.position, 1.0f);
        iTween.RotateTo(gameObject, _endpos.localEulerAngles, 1.0f);
    }

    public void OnBackToMainClicked()
    {
        MoveCameraTween(mainTransform); 
        //StartCoroutine(MoveCamera(mainTransform));
    }

    IEnumerator MoveCamera(Transform _endPos)
    {
        transform.position = Vector3.Lerp(transform.position, _endPos.position, 5.0f * Time.deltaTime);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, _endPos.localRotation, 5.0f * Time.deltaTime);
        Debug.Log(_endPos.localEulerAngles);
        yield return new WaitForEndOfFrame();

        StartCoroutine(MoveCamera(_endPos));
    }
}
