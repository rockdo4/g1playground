using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAttack : AttackDefinition
{
    public int reqMana;
    public float damageFigure;
    public float criticalChance;
    public float criticalDamage;
    public float range;
    public float lifeTime;
    public bool isKnockbackable;
    public bool isStunnable;
    public float stunTime;
    public float CoolDown;

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
