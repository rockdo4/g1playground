using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackedEffect : MonoBehaviour, IAttackable
{
    public HitEffect effect;

    public void OnAttack(GameObject attacker, Attack attack, Vector3 attackPos)
    {
        var eff = attack.IsCritical ? string.Format(effect.criticalHitEffect) : string.Format(effect.hitEffect) ;
        var feect = GameManager.instance.effectManager.GetEffect(eff);
        feect.transform.position = attackPos;
        GameManager.instance.effectManager.ReturnEffectOnTime(eff,feect, 1);
    }
}
