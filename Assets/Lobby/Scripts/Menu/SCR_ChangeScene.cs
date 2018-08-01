using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SCR_ChangeScene : MonoBehaviour
{
    public void ChangeScene(int _sceneIndex)
    {
        if (FindObjectOfType<CustomLobbyManager>())
        {
            Destroy(FindObjectOfType<CustomLobbyManager>().gameObject);
        }
        SceneManager.LoadScene(_sceneIndex);
    }

    public void ChangeSceneAndSignOut(int _sceneIndex)
    {
        if (FindObjectOfType<CustomLobbyManager>())
        {
            FindObjectOfType<CustomLobbyManager>().ShutDownNetworkManager(_sceneIndex);
        }

        SceneManager.LoadScene(_sceneIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
