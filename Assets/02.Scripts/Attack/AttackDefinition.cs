using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackDefinition
{
    public float coolDown;
    public enum Types
    {
        Basic,
        Skill,
    }
    public abstract Attack CreateAttack(Status attacker, Status defenser);
}
