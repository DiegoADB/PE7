using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SRC_AnimatoinItems : MonoBehaviour
{
    float posicionInicial;
    float posicionFinal;
    bool xd = false;
    private void Start()
    {
        posicionInicial = this.transform.position.y;
        posicionFinal = this.transform.position.y - 0.5f;
    }

    // Update is called once per frame
    void Update ()
    {
        this.transform.Rotate(Vector3.up * Time.deltaTime * 10);
        if(!xd)
        {
            xd = true;
            bajar();
        }
	}

    void bajar()
    {
        Mathf.Lerp(this.transform.position.y, posicionFinal, 0.5f);
    }
}
