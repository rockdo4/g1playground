using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : AttackDefinition
{
    public override Attack CreateAttack(Status attacker, Status defenser)    // temporary formula
    {
        var isCritical = Random.value < attacker.FinalValue.meleeCriChance;
        float damage = attacker.FinalValue.meleePower;
        if (isCritical)
            damage *= attacker.FinalValue.meleeCriDamage;
        damage -= defenser.FinalValue.meleeDef;
        return new Attack((int)damage);
    }

    public override void ExecuteAttack(GameObject attacker, GameObject defender, Vector3 attackPos)
    {
        if (attacker == null || defender == null)
            return;
        var aStat = attacker.GetComponent<Status>();
        var dStat = defender.GetComponent<Status>();
        if (aStat == null || dStat == null)
            return;

        var attack = CreateAttack(aStat, dStat);

        var attackables = defender.GetComponents<IAttackable>();
        foreach (var attackable in attackables)
        {
            attackable.OnAttack(attacker, attack, attackPos);
        }
    }
}
