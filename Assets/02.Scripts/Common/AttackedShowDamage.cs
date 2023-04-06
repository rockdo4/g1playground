using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackedShowDamage : MonoBehaviour, IAttackable
{
    public void OnAttack(GameObject attacker, Attack attack, Vector3 attackPos)
    {
        var effect = GameManager.instance.effectManager.GetEffect("Damage").GetComponent<DamageEffect>();
        effect.OnDamage(attackPos, attack.Damage, CompareTag("Player") ? Color.red : Color.white, attack.IsCritical);
    }
}
