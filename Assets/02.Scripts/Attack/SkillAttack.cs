using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAttack : AttackDefinition
{
    public float damageFigure = 1.2f;
    public float criticalChance = 0.5f;
    public float criticalDamage = 2f;
    public float range = 10f;
    public float speed = 5f;
    public bool isStunnable = true;
    public float stunTime = 2f;
    public float CoolDown = 3f;

    public void SetSkill(string id)
    {
        // loadFromSkillTable
    }

    public override Attack CreateAttack(Status attacker, Status defenser)
    {
        var criticalChance = attacker.CriticalChance + this.criticalChance;
        var isCritical = Random.value < criticalChance;
        float damage = attacker.SkillPower;
        if (isCritical)
        {
            damage *= (attacker.CriticalDamage + this.criticalDamage);  // temporary
        }
        return new Attack((int)damage);
    }

    public override void ExecuteAttack(GameObject attacker, GameObject defender)
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
            attackable.OnAttack(attacker, attack);
        }
        if (isStunnable)
        {
            var stunnables = defender.GetComponents<IStunnable>();
            foreach (var stunnable in stunnables)
            {
                stunnable.OnStunned(stunTime);
            }
        }
    }
}
