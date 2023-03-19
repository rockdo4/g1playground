using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAttack : AttackDefinition
{
    public string id;
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
    public bool isOnOffSkill = false;
    public string fireSoundEffect;
    public string inUseSoundEffect;
    public string hitSoundEffect;

    public void SetSkill(string id)
    {
        // loadFromSkillTable
    }

    public virtual void Update() { }

    public override Attack CreateAttack(Status attacker, Status defenser)   // temporary formula
    {
        var criticalChance = attacker.FinalValue.skillCriChance + this.criticalChance;
        var isCritical = Random.value < criticalChance;
        float damage = attacker.FinalValue.skillPower * damageFigure;
        if (isCritical)
            damage *= (attacker.FinalValue.skillCriDamage + this.criticalDamage);
        damage -= defenser.FinalValue.skillDef;
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
