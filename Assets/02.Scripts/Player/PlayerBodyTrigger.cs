using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyTrigger : MonoBehaviour
{
    public float damagedDelay;
    private float timer;
    private bool isOnDelay = false;
    public float damageRate = 0.2f;
    private Vector3 bodyCenter = new Vector3(0f, 0.5f, 0f);

    private void Update()
    {
        if (isOnDelay)
        {
            timer += Time.deltaTime;
            if (timer > damagedDelay)
            {
                isOnDelay = false;
                timer = 0f;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isOnDelay)
            return;
        var enemyStat = other.GetComponentInParent<Status>();
        if (enemyStat == null)
            return;
        var damage = (int)(damageRate * (enemyStat.FinalValue.meleePower > enemyStat.FinalValue.skillPower ? enemyStat.FinalValue.meleePower : enemyStat.FinalValue.skillPower));
        Attack.CC newCC = Attack.CC.None;
        newCC.knockBackForce = 7f;
        Attack attack = new Attack(damage, newCC, false);
        var attackables = transform.parent.GetComponents<IAttackable>();
        var attacker = other.transform.parent.gameObject;
        var bodyPos = transform.position + bodyCenter;
        var attackPos = other.ClosestPoint(bodyPos);
        foreach (var attackable in attackables)
        {
            attackable.OnAttack(attacker, attack, attackPos);
        }
        isOnDelay = true;
    }
}
