using System.Collections;
using System.Collections.Generic;

public class Attack
{
    public int Damage { get; private set; }
    public bool IsKnockBackable { get; private set; }
    // addtional option

    public Attack(int damage, bool isKnockBackable = true)
    {
        Damage = damage;
        IsKnockBackable = isKnockBackable;
    }
}
