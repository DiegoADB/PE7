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
        if (FindObjectOfType<SCR_CustomLobbiesLobby>())
        {
            Destroy(FindObjectOfType<SCR_CustomLobbiesLobby>().gameObject);
        }
        SceneManager.LoadScene(_sceneIndex);
    }

    public void ChangeSceneAndSignOut(int _sceneIndex)
    {
        if (FindObjectOfType<CustomLobbyManager>())
        {
            FindObjectOfType<CustomLobbyManager>().ShutDownNetworkManager(_sceneIndex);
        }
        if (FindObjectOfType<SCR_CustomLobbiesLobby>())
        {
            FindObjectOfType<SCR_CustomLobbiesLobby>().ShutDownNetworkManager(_sceneIndex);
        }

        SceneManager.LoadScene(_sceneIndex);
    }
    public void ChangeModel()
    {
        CustomLobbyManager.pingo = FindObjectOfType<InputField>().text;
        if (CustomLobbyManager.pingo == "")
            CustomLobbyManager.pingo = "Swfit";
        Debug.Log(CustomLobbyManager.pingo);
    }
}
