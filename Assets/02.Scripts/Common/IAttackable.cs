using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackable
{
    public void OnAttack(GameObject attacker, Attack attack, Vector3 attackPos);
}
