using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "StraightSpell", menuName = "SkillAttack/StraightSpell")]
public class StraightSpell : SkillAttack
{
    public float lifeTime;
    public float range;
    public string projectileId;
    public int maxCount;
    public float interval;

    public void Fire(GameObject attacker, Vector3 startPos, Vector3 direction)
    {
        var aStat = attacker.GetComponent<Status>();
        MonoBehaviour monoBehaviour = attacker.GetComponent<MonoBehaviour>();
        if (aStat == null || aStat.CurrMp < reqMana)
            return; 
        aStat.CurrMp -= reqMana;
        if (attacker.CompareTag("Player"))
            aStat.SetMpUi();
        monoBehaviour.StartCoroutine(CoFire(attacker, startPos, direction));
    }

    private IEnumerator CoFire(GameObject attacker, Vector3 startPos, Vector3 direction)
    {
        int count = 0;
        while (true)
        {
            var projectile = GameManager.instance.attackColliderManager.Get<Projectile>(projectileId);
            projectile.OnCollided = ExecuteAttack;
            projectile.Fire(attacker, startPos, direction, range, lifeTime, false, false, fireSoundEffect, inUseSoundEffect, hitSoundEffect);
            if (++count >= maxCount)
                break;
            yield return new WaitForSeconds(interval);
        }
    }
}
