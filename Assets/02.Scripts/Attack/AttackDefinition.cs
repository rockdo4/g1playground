using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackDefinition : ScriptableObject
{
    public float coolDown;
    public enum Types
    {
        Basic,
        Skill,
    }
    public Types Type { get; protected set; }

    public abstract Attack CreateAttack(Status attacker, Status defenser);
    public abstract void ExecuteAttack(GameObject attacker, GameObject defender);
}
