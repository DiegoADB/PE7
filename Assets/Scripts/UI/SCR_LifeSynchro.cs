using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_LifeSynchro : MonoBehaviour {

    public GameObject[] lifePrefabs;
    public int playerTotalHP;

    public int playerMaxHP;

    public int currentHP;
    private int tempHPValue;


    public int loopInt = 0;
    
	void Start () {

        playerMaxHP = (int)GetComponent<SCR_CharacterStats>().playerHP;
        tempHPValue = playerMaxHP;
        currentHP = (int)GetComponent<SCR_CharacterStats>().playerHP;

    }

    void Update ()
    {
        currentHP = (int)GetComponent<SCR_CharacterStats>().playerHP;

        if (currentHP != tempHPValue)
        {
            tempHPValue = currentHP;
            NewSynchro();
        }
	}

    //Call on playerHP change
    void NewSynchro()
    {
        if(currentHP <= 0)
        {
            ToggleHealthPrefabs(0, true, false);
        }

        if(currentHP >= playerMaxHP)
        {
            ToggleHealthPrefabs(0, false, true);
        }

        if(currentHP != 0 && currentHP != playerMaxHP && currentHP > 0 && currentHP < playerMaxHP)
        {
            float indexMagicNumber = playerMaxHP / 6;
            int prefabIndex = Mathf.RoundToInt((currentHP / indexMagicNumber));

            Debug.Log("prefabIndex: " + prefabIndex);
            ToggleHealthPrefabs(prefabIndex, false, false);
        }

    }

    public void ToggleHealthPrefabs(int _prefabIndex, bool _allOff, bool _allOn)
    {
        for(int i = 0; i < lifePrefabs.Length; i++)
        {
            lifePrefabs[i].SetActive(false);
        }

        lifePrefabs[0].SetActive(true);

        if (!_allOff && !_allOn)
        {
            for (int i = 0; i < _prefabIndex; i++)
            {
                lifePrefabs[i].SetActive(true);
            }
        }

        if(_allOn)
        {
            for(int i = 0; i < lifePrefabs.Length; i++)
            {
                lifePrefabs[i].SetActive(true);
            }
        }

        if(_allOff)
        {
            for (int i = 0; i < lifePrefabs.Length; i++)
            {
                lifePrefabs[i].SetActive(false);
            }
        }
    }
}
