using System.Collections;
using System.Collections.Generic;

public class Attack
{
    public int Damage { get; private set; }
    public float KnockBackForce { get; private set; }
    // addtional option

    public Attack(int damage, float knockBackForce = 0f)
    {
        Damage = damage;
        KnockBackForce = knockBackForce;
    }
}
