using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoomerangSpell", menuName = "SkillAttack/BoomerangSpell")]
public class BoomerangSpell : SkillAttack
{
    public string projectileId;

    public void Fire(GameObject attacker, Vector3 startPos, Vector3 direction)
    {
        var aStat = attacker.GetComponent<Status>();
        if (aStat == null || aStat.CurrMp < reqMana)
            return;
        aStat.CurrMp -= reqMana;
        if (attacker.CompareTag("Player"))
            aStat.SetMpUi();
        var projectile = GameManager.instance.attackColliderManager.Get<Projectile>(projectileId);
        projectile.OnCollided = ExecuteAttack;
        projectile.Fire(attacker, startPos, direction, range, lifeTime, true, true, fireSoundEffect, inUseSoundEffect, hitSoundEffect);
    }
}
