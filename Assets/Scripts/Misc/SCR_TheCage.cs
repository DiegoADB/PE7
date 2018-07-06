using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SCR_TheCage : MonoBehaviour {

    public float waitTime;

    public GameObject cage;
    public Text countdownText;


    private void FixedUpdate()
    {
        waitTime -= Time.fixedDeltaTime;
        countdownText.text = Mathf.FloorToInt(waitTime).ToString();
        if (Mathf.FloorToInt(waitTime) == 0)
        {
            countdownText.text = "GO!";
        }
        if (waitTime <= 0)
        {
            countdownText.text = "";
            Destroy(cage);
            Destroy(gameObject);
        }
    }
}
