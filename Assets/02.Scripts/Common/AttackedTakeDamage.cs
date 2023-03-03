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
        stat.currHp -= attack.Damage;
        if (CompareTag("Player"))
            stat.SetHpUi();
        if (stat.currHp <= 0)
        {
            stat.currHp = 0;
            var destructibles = GetComponents<IDestructible>();
            foreach (var destructible in destructibles)
            {
                destructible.OnDestroyObj();
            }
        }
    }
}
