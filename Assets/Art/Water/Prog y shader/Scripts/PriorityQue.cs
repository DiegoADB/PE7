using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PriorityQue : MonoBehaviour {

    //TheType myArray = new TheType[lengthOfArray];  // declaration
    public int[] myNumbers;                 // declaration example using ints
    public GameObject[] enemies;
    public QueuePriority monster;

    public GameObject butonPlayer;
    public Text textTurn;


    //public List<badguy> badguys = new List<badguy>();



	public List<int> Turno = new List<int>();                 // declaration
    //List<int> someNumbers = new List<int>();            


	// a real-world example of declaring a List of 'ints'
    //List<GameObject> enemies = new List<GameObject>();
    // declaration example using GameObjects
    //int howBig = myArray.Length;               // get the length of the array
    //myArray[i] = newValue;                     // set a value at position i
    //TheType thisValue = myArray[i];            // get a value from position i
    // System.Array.Resize(ref myArray, size);    //resize array





    // Use this for initialization
    void Start () {



        //badguys.Add(new badguy("enemy1", 50));
        //badguys.Add(new badguy("enemy2", 150));
        //badguys.Add(new badguy("enemy3", 250));

        monster = GetComponentInChildren<QueuePriority>();

        butonPlayer.SetActive(false);


    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space))
        {

            //weapons.Add("knife");
            Turno.Add(1);
            Turno.Add(2);


        }
        if (Input.GetKeyDown(KeyCode.Return)) {
            //Turno.RemoveAt(0);

            Turno.RemoveAt(0);
        }
        //Si no hay turnos,, no hago nada
        if (Turno.Count == 0)
        {
            textTurn.text = "turno de: nadie";
            butonPlayer.SetActive(false);
            return;
        }
        
        if (Turno[0] == 1 )
        {
            Debug.Log("turno del jugador");
            textTurn.text = "turno de: jugador";
            butonPlayer.SetActive(true);

        } else if (Turno[0] == 2)
        {
            Debug.Log("turno del enemigo");
            textTurn.text = "turno de: Enemigo";

            monster.attack = true;

        }



    }
}
