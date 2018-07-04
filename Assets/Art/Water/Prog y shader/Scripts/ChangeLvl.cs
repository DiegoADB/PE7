using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLvl : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Player touch");

            StartCoroutine(ChangeLevel());
            //ChangeLevel();
        }

    }

    IEnumerator ChangeLevel()
    {
        //yield return new WaitForSeconds(0.6f);
        float fadeTime = GameObject.Find("_GM").GetComponent<Fading>().BeginFade(1);
        Application.LoadLevel(Application.loadedLevel + 1);

        yield return new WaitForSeconds(fadeTime);
    }

}
