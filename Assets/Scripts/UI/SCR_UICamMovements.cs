using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SCR_UICamMovements : MonoBehaviour {

    public Transform mainTransform;
    public Transform optionsTransform;

    public enum ItemList
    {
        Switcheroo = 0,
        RedShell
    }

    private void Start()
    {
        transform.position = mainTransform.position;
        transform.rotation = mainTransform.rotation;
    }

    public void OnOptionsClicked()
    {
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
    }

    public void OnSinglePlayerClicked()
    {
        SceneManager.LoadSceneAsync("SinglePlayerScene.wut");
    }

    public void OnMultiPlayerClicked()
    {
        SceneManager.LoadSceneAsync("MultiPlayerScene.wut");
    }

    //Slider changes on volume options
}
