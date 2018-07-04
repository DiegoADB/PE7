using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class badguy : IComparable<badguy>
{


    public string name;
    public int power;

    public badguy(string newName, int NewPower)
    {

        name = newName;
        power = NewPower;
    }
    public int CompareTo(badguy other)
    {

        if(other == null)
        {
            return 1;
        }
        return power - other.power;
    }
}
