using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    protected int maxHp;
    protected int currHp;

    private void Start()
    {
        // load from dataTable
        currHp = maxHp;
    }

    public void OnDamage(int dmg)
    {
        currHp -= dmg;
        if (currHp < 0)
            OnDie();
    }

    public virtual void OnDie() { }
}
