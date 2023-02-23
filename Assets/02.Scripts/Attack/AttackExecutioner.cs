using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackExecutioner
{
    protected AttackDefinition attackDef;

    public AttackExecutioner(AttackDefinition.Types attackDefType, string skillId = "")
    {
        switch (attackDefType)
        {
            case AttackDefinition.Types.Basic:
                attackDef = new BasicAttack();
                break;
            case AttackDefinition.Types.Skill:
                attackDef = new SkillAttack();
                ((SkillAttack)attackDef).SetSkill(skillId);
                break;
        }
    }

    public virtual void ExecuteAttack(GameObject attacker, GameObject defender) { }
}
