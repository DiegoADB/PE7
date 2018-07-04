 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour {

    public float height;
    public float time;


    public float scrollSpeed = 0.5F;
    public Renderer rend;

    // Use this for initialization
    void Start () {
        rend = GetComponent<Renderer>();

        iTween.MoveBy(this.gameObject,iTween.Hash("y",height,"time",time,"looptype","pingpong","easetype",iTween.EaseType.easeInOutSine));
		
	}
	
	// Update is called once per frame
	void Update () {
        float offset = Time.time * scrollSpeed;
        rend.material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
    }
}
