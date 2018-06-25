using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_PlayerPrefs : MonoBehaviour
{
    public static SCR_PlayerPrefs instance = null;
    public GameObject[] penguins;
    public float currency;
    public int myPenguin;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
}
