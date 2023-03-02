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
        if (aStat == null || aStat.currMp < reqMana)
            return;
        aStat.currMp -= reqMana;
        var projectile = GameManager.instance.projectileManager.Get(projectileId);
        projectile.OnCollided = ExecuteAttack;
        projectile.Fire(attacker, startPos, direction, range, lifeTime, true, true);
    }
}
