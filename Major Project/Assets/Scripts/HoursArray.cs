using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HoursArray
{
    public int[] hours;

    public HoursArray(int[] hours)
    {
        this.hours = new int[24];
    }
}
