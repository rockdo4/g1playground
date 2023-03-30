using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxMp : BarUI
{
    private int maxStat;
    protected void Awake()
    {
        base.Awake();
        maxValue = maxStat;
    }
}
