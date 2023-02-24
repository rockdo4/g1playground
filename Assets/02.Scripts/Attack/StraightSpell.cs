using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "StraightSpell", menuName = "SkillAttack/StraightSpell")]
public class StraightSpell : SkillAttack
{
    private IObjectPool<Projectile> projectilePool;
    public Projectile projectilePrefab;

    public StraightSpell()
    {
        projectilePool = new ObjectPool<Projectile>(CreateProjectile, OnGetProjectile, OnReleaseProjectile, OnDestroyProjectile);
    }

    private Projectile CreateProjectile()
    {
        var projectile = Object.Instantiate(projectilePrefab);
        projectile.SetPool(projectilePool);
        projectile.OnCollided = ExecuteAttack;
        return projectile;
    }
    private void OnGetProjectile(Projectile projectile) => projectile.gameObject.SetActive(true);
    private void OnReleaseProjectile(Projectile projectile) => projectile.gameObject.SetActive(false);
    private void OnDestroyProjectile(Projectile projectile) => Object.Destroy(projectile.gameObject);

    public void Fire(GameObject attacker, Vector3 startPos, Vector3 direction)
    {
        var projectile = projectilePool.Get();
        projectile.Fire(attacker, startPos, direction, range, speed);
    }
}
