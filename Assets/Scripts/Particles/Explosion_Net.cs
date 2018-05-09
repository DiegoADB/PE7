using UnityEngine;
using UnityEngine.Networking;

public class Explosion_Net : NetworkBehaviour
{
    public float timeToExplode;
    private float theTime = 0;

    void FixedUpdate()
    {
        theTime += Time.fixedDeltaTime;
        if (theTime > timeToExplode)
            NetworkServer.Destroy(gameObject);
    }
}

