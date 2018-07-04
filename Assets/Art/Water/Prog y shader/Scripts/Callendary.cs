using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Callendary : MonoBehaviour {

    public GameObject obj1;
    public GameObject obj2;
    public GameObject obj3;

    public bool bool1;
    public bool bool2;
    public bool bool3;

    ///move stufs
    ///
    public GameObject hole;
    public GameObject wall;

    private Vector3 starPos1;
    private Vector3 starPos2;
    private Vector3 starPos3;


    private Vector3 endPos;

    private float distance = 10f;

    private float lerpTime = 50f;

    private float currentLerpTime = 0;

    private bool keyHit = false;



    // Use this for initialization
    void Start () {

        obj1.SetActive(false);
        obj2.SetActive(false);
        obj3.SetActive(false);
        hole.SetActive(false);

        endPos = transform.position + Vector3.down * distance;




    }

    // Update is called once per frame
    void Update () {

        if((bool1==true)&&(bool2==true)&&(bool3==true))
        {
            obj1.SetActive(true);
            obj2.SetActive(true);
            obj3.SetActive(true);
            StartCoroutine(SetPieces());


        }
       

            starPos1 = obj1.transform.position;
            starPos2 = obj2.transform.position;
            starPos3 = obj3.transform.position;

        if (Input.GetKeyDown(KeyCode.Alpha1))
            {

                keyHit = true;

            }
            if (keyHit == true)
            {

                currentLerpTime += Time.deltaTime;
                if (currentLerpTime >= lerpTime)
                {
                    currentLerpTime = lerpTime;
                }
                float Perc = currentLerpTime / lerpTime;
                obj1.transform.position = Vector3.Lerp(starPos1, endPos, Perc);
                obj2.transform.position = Vector3.Lerp(starPos2, endPos, Perc);
                obj3.transform.position = Vector3.Lerp(starPos3, endPos, Perc);


            }


    }




    void OnTriggerStay(Collider other)
    {
        
        if(other.gameObject.tag=="Player")
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("click");
                bool1 = other.GetComponent<Movement>().Pick1B;
                bool2 = other.GetComponent<Movement>().Pick2B;
                bool3 = other.GetComponent<Movement>().Pick3B;

            }

        }

    }

    IEnumerator SetPieces()
    {
        //obj1.GetComponent<MovePieces>().keyHit = true;
        
        yield return new WaitForSeconds(5);
        keyHit = true;
        StartCoroutine(Destryo());

    }

    IEnumerator Destryo()
    {
        yield return new WaitForSeconds(5);
        hole.SetActive(true);
        Destroy(gameObject);
    }

    }

