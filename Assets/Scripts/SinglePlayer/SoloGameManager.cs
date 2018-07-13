using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoloGameManager : MonoBehaviour {

    public GameObject[] StartPos;
    public SCR_TheCage cage;
    public GameObject myPlayer;
    public GameObject theEnemy;

    GameObject thePlayer;
    private void Start()
    {
        
        //Invoke("Despabila", cage.waitTime);
        for(int i = 0; i< StartPos.Length;i++)
        {
            if (i == 0)
            {
                thePlayer = Instantiate(myPlayer, StartPos[i].transform.position, Quaternion.identity);
                //thePlayer.GetComponent<SCR_CharacterMotor_Net>().enabled = false;
                //thePlayer.SetActive(true);
            }
            else
            {
                Instantiate(theEnemy, StartPos[i].transform.position, Quaternion.identity);
            }
        }
    }

	
    void Despabila()
    {
        thePlayer.GetComponent<SCR_CharacterMotor_Net>().enabled = true;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
