using UnityEngine;

public class Explosion_Solo : MonoBehaviour
{
    public float timeToExplode;
    private float theTime = 0;

    void FixedUpdate()
    {
        theTime += Time.fixedDeltaTime;
        if (theTime > timeToExplode)
        {
            Destroy(gameObject);
        }
    }
}

