using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDefBar : BarUI
{
    private int maxStat;
    protected void Awake()
    {
        base.Awake();
        maxValue = maxStat;
    }
}
