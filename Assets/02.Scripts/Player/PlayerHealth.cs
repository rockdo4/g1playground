using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health, IGrowable
{
    private int baseHp;
    private int growthHp;

    private void Start()
    {
        // load from dataTable (baseHp = healthTable.maxHp)
        maxHp = baseHp;
    }

    public override void OnDie()
    {
        // game over
    }

    public void ApplyLevel(int level)
    {
        var newMaxHp = baseHp + level * growthHp;
        currHp = newMaxHp - maxHp;
        maxHp = newMaxHp;
    }
}
