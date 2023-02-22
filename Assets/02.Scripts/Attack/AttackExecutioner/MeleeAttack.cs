using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : AttackExecutioner
{
    public MeleeAttack(AttackDefinition.Types attackDefType, string skillId = "") : base(attackDefType, skillId) { }

    public void OnCollided(GameObject attacker, GameObject defender)
    {
        if (attacker == null || defender == null)
            return;

        var aStat = attacker.GetComponent<Status>();
        var dStat = defender.GetComponent<Status>();
        var attack = attackDef.CreateAttack(aStat, dStat);

        var attackables = defender.GetComponentsInChildren<IAttackable>();
        foreach (var attackable in attackables)
        {
            attackable.OnAttack(attacker, attack);
        }
    }
}
