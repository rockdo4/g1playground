using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackedTakeDamage : MonoBehaviour, IAttackable
{
    private Status stat;

    private void Awake()
    {
        stat = GetComponent<Status>();
    }

    public void OnAttack(GameObject attacker, Attack attack)
    {
        stat.currHp -= attack.Damage;
        if (stat.currHp <= 0)
        {
            stat.currHp = 0;
            var destructibles = GetComponents<IDestructible>();
            foreach (var destructible in destructibles)
            {
                destructible.OnDestroy();
            }
        }
    }
}
