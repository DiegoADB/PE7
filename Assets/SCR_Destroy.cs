using UnityEngine;
using UnityEngine.Networking;

public class SCR_Destroy : MonoBehaviour {

    private void Start()
    {
        Invoke("DestroyMe", 5);
    }
    void DestroyMe()
    {
        Destroy(gameObject);
    }
}
