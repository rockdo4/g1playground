using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackDefinition
{
    public void ExecuteAttack();
    public Attack CreateAttack(Status attacker, Status defenser);
}
