using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX_waterfall : MonoBehaviour {
    public AudioSource[] audio_;
	// Use this for initialization
	void Start ()
    {
        audio_[0].GetComponents<AudioSource>();
        audio_[1].GetComponents<AudioSource>();
        audio_[0].Play();
        audio_[1].Play();
    }
	
	// Update is called once per frame
	void Update () {
        
    }
}
