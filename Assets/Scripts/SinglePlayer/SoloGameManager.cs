using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoloGameManager : MonoBehaviour {

    public GameObject[] StartPos;
    public GameObject[] myPlayer;
    public GameObject[] theEnemy;


    GameObject thePlayer;
    private void Start()
    {
        Debug.Log(SceneManager.GetActiveScene());
        //Invoke("Despabila", cage.waitTime);
        for(int i = 0; i< StartPos.Length;i++)
        {
            if (i == 0)
            {
                thePlayer = Instantiate(myPlayer[Random.Range(0, 4)], StartPos[i].transform.position, Quaternion.identity);
                //thePlayer.GetComponent<SCR_CharacterMotor_Net>().enabled = false;
                //thePlayer.SetActive(true);
            }
            else
            {
                Instantiate(theEnemy[Random.Range(0,4)], StartPos[i].transform.position, Quaternion.identity).transform.name = "Pinguino_IA_" + i ;
            }
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }
}
