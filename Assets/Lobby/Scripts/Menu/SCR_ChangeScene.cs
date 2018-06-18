using UnityEngine;
using UnityEngine.SceneManagement;

public class SCR_ChangeScene : MonoBehaviour
{
    public void ChangeScene(int _sceneIndex)
    {
        SceneManager.LoadScene(_sceneIndex);
    }
}
