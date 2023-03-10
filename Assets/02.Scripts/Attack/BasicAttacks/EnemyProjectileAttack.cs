using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "EnemyProjectileAttack", menuName = "BasicAttack/EnemyProjectileAttack")]
public class EnemyProjectileAttack : BasicAttack
{
    public string projectileId;
    public float range;
    public float lifeTime;

    public void Fire(GameObject attacker, Vector3 startPos, Vector3 direction)
    {
        var aStat = attacker.GetComponent<Status>();
        if (aStat == null )
            return;
        var projectile = GameManager.instance.attackColliderManager.Get<Projectile>(projectileId);
        projectile.OnCollided = ExecuteAttack;
        projectile.Fire(attacker, startPos, direction, range, lifeTime, false, false);
    }
}
