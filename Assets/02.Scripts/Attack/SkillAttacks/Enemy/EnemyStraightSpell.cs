using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStraightSpell", menuName = "SkillAttack/EnemyStraightSpell")]
public class EnemyStraightSpell : SkillAttack
{
    public float lifeTime;
    public float range;
    public string projectileId;

    private void Awake()
    {
        //if (!string.IsNullOrEmpty(id))
        //    SetData(id);
    }

    public void Fire(GameObject attacker, Vector3 startPos, Vector3 direction)
    {
        var aStat = attacker.GetComponent<Status>();
        MonoBehaviour monoBehaviour = attacker.GetComponent<MonoBehaviour>();
        if (aStat == null)
            return;

        var projectile = GameManager.instance.attackColliderManager.Get<Projectile>(projectileId);
        projectile.OnCollided = ExecuteAttack;
        projectile.Fire(attacker, startPos, direction, range, lifeTime, false, false, fireSoundEffect, inUseSoundEffect, hitSoundEffect);
    }
}
