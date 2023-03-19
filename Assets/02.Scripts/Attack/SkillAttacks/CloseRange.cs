using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CloseRange", menuName = "SkillAttack/CloseRange")]
public class CloseRange : SkillAttack
{
    public bool isAttachedToAttacker = false;
    public string rangeColliderId;
    public bool isContinuousAttack = false;
    public float interval;

    public void Fire(GameObject attacker, Transform attackPivot)
    {
        var aStat = attacker.GetComponent<Status>();
        if (aStat == null || aStat.CurrMp < reqMana)
            return;
        aStat.CurrMp -= reqMana;
        if (attacker.CompareTag("Player"))
            aStat.SetMpUi();
        var rangeCollider = GameManager.instance.attackColliderManager.Get<RangeCollider>(rangeColliderId);
        rangeCollider.OnCollided = ExecuteAttack;
        if (isAttachedToAttacker)
            rangeCollider.Fire(attacker, attackPivot, true, lifeTime, isContinuousAttack, interval, fireSoundEffect, inUseSoundEffect, hitSoundEffect);
        else
            rangeCollider.Fire(attacker, attackPivot.position, attackPivot.forward, true, lifeTime, isContinuousAttack, interval, fireSoundEffect, inUseSoundEffect, hitSoundEffect);
    }
}
