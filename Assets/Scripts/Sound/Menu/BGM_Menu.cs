using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM_Menu : MonoBehaviour {
    public AudioSource[] _audio;

	// Use this for initialization
	void Start ()
    {
        _audio[0].GetComponents<AudioSource>();
        _audio[1].GetComponents<AudioSource>();
        _audio[2].GetComponents<AudioSource>();
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void Menu_boton()
    {
        _audio[Random.Range(0, 2)].Play();
    }
}
