using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack
{
    public int Damage { get; private set; }
    // addtional option

    public Attack(int damage)
    {
        Damage = damage;
    }
}
