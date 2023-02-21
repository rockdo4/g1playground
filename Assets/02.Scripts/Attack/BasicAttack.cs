using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : IAttackDefinition
{
    public virtual void ExecuteAttack() { }

    public Attack CreateAttack(Status attacker, Status defenser)
    {
        var isCritical = Random.value < attacker.CriticalChance;
        float damage = attacker.AtkPower;
        if (isCritical)
            damage *= attacker.CriticalDamage;
        return new Attack((int)damage);
    }
}
