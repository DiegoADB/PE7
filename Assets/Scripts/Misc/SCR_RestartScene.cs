using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SCR_RestartScene : MonoBehaviour {

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
            SceneManager.LoadSceneAsync(0);
    }
}
