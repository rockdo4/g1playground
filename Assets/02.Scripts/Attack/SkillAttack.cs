using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAttack : AttackDefinition
{
    protected float damageFigure;
    protected float criticalChance;
    protected float criticalDamage;
    protected bool isStunnable;
    protected float stunTime;
    protected float coolDown;

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
}
