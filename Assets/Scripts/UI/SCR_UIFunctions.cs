using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_UIFunctions : MonoBehaviour {

	public void OnExitClicked()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

}
