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

    public void OnAttack(GameObject attacker, Attack attack, Vector3 attackPos)
    {
        stat.CurrHp -= attack.Damage;
        if (stat.CurrHp <= 0)
        {
            stat.CurrHp = 0;
            var destructibles = GetComponents<IDestructible>();
            foreach (var destructible in destructibles)
            {
                destructible.OnDestroyObj();
            }
        }
    }
}
