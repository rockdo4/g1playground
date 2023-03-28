using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackedEffect : MonoBehaviour, IAttackable
{
    public HitEffect effect;

    public void OnAttack(GameObject attacker, Attack attack, Vector3 attackPos)
    {
        //var eff = attack.IsCritical ? string.Format(effect.hitEffect) : string.Format(effect.criticalHitEffect);
        //var feect=GameManager.instance.effectManager.GetEffect(eff);
        //feect.transform.position = transform.position;
    }
}
